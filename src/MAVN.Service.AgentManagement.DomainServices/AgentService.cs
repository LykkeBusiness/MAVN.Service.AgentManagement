using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using MAVN.Service.AgentManagement.Domain.Entities.Agents;
using MAVN.Service.AgentManagement.Domain.Entities.Registration;
using MAVN.Service.AgentManagement.Domain.Entities.Requirements;
using MAVN.Service.AgentManagement.Domain.Exceptions;
using MAVN.Service.AgentManagement.Domain.Extensions;
using MAVN.Service.AgentManagement.Domain.Repositories;
using MAVN.Service.AgentManagement.Domain.Services;
using Lykke.Service.CustomerProfile.Client;
using Lykke.Service.CustomerProfile.Client.Models.Enums;
using Lykke.Service.CustomerProfile.Client.Models.Requests;
using Lykke.Service.MAVNPropertyIntegration.Client;
using Lykke.Service.MAVNPropertyIntegration.Client.Enums;
using Lykke.Service.MAVNPropertyIntegration.Client.Models;
using Lykke.Service.MAVNPropertyIntegration.Client.Models.Requests;
using Lykke.Service.NotificationSystem.SubscriberContract;
using Lykke.Service.PrivateBlockchainFacade.Client;

namespace MAVN.Service.AgentManagement.DomainServices
{
    public class AgentService : IAgentService
    {
        private readonly string _componentSourceName;
        private readonly string _agentApprovedPushTemplateId;
        private readonly string _agentRejectedPushTemplateId;
        private readonly string _agentRejectedEmailSubjectTemplateId;
        private readonly string _agentRejectedEmailTemplateId;
        private readonly IRequirementsService _requirementsService;
        private readonly IAgentRepository _agentRepository;
        private readonly IImageRepository _imageRepository;
        private readonly ICustomerProfileClient _customerProfileClient;
        private readonly IPrivateBlockchainFacadeClient _pbfClient;
        private readonly IRabbitPublisher<PushNotificationEvent> _pushNotificationPublisher;
        private readonly IRabbitPublisher<EmailMessageEvent> _emailNotificationPublisher;
        private readonly IMAVNPropertyIntegrationClient _tokenPropertyIntegrationClient;
        private readonly ISettingsService _settingsService;
        private readonly ILog _log;

        public AgentService(
            string agentApprovedPushTemplateId,
            string agentRejectedPushTemplateId,
            string agentRejectedEmailSubjectTemplateId,
            string agentRejectedEmailTemplateId,
            IRequirementsService requirementsService,
            IAgentRepository agentRepository,
            IImageRepository imageRepository,
            ICustomerProfileClient customerProfileClient,
            IPrivateBlockchainFacadeClient pbfClient,
            IRabbitPublisher<PushNotificationEvent> pushNotificationPublisher,
            IRabbitPublisher<EmailMessageEvent> emailNotificationPublisher,
            IMAVNPropertyIntegrationClient tokenPropertyIntegrationClient,
            ISettingsService settingsService,
            ILogFactory logFactory)
        {
            _agentApprovedPushTemplateId = agentApprovedPushTemplateId;
            _agentRejectedPushTemplateId = agentRejectedPushTemplateId;
            _agentRejectedEmailSubjectTemplateId = agentRejectedEmailSubjectTemplateId;
            _agentRejectedEmailTemplateId = agentRejectedEmailTemplateId;
            _requirementsService = requirementsService;
            _agentRepository = agentRepository;
            _imageRepository = imageRepository;
            _customerProfileClient = customerProfileClient;
            _pbfClient = pbfClient;
            _pushNotificationPublisher = pushNotificationPublisher;
            _emailNotificationPublisher = emailNotificationPublisher;
            _tokenPropertyIntegrationClient = tokenPropertyIntegrationClient;
            _settingsService = settingsService;
            _log = logFactory.CreateLog(this);
            _componentSourceName = $"{AppEnvironment.Name} - {AppEnvironment.Version}";
        }

        public Task<Agent> GetByCustomerIdAsync(Guid customerId)
        {
            return _agentRepository.GetByCustomerIdAsync(customerId);
        }

        public Task<IReadOnlyCollection<Agent>> GetByCustomerIdsAsync(Guid[] customerIds)
        {
            return _agentRepository.GetByCustomerIdsAsync(customerIds);
        }

        public async Task RegisterAsync(RegistrationForm registrationForm)
        {
            var agent = await _agentRepository.GetByCustomerIdAsync(registrationForm.CustomerId);

            if (agent != null)
            {
                if (agent.Status == AgentStatus.ApprovedAgent)
                    throw new AlreadyApprovedException();

                if (agent.Status == AgentStatus.Rejected)
                    throw new SalesforceAccountAlreadyExistsException();
            }

            if (!await ValidateEmailAsync(registrationForm.CustomerId))
                throw new EmailNotVerifiedException();

            if (!await ValidateNumberOfTokensAsync(registrationForm.CustomerId))
                throw new NotEnoughTokensException();

            var customerProfile = await _customerProfileClient.CustomerProfiles
                .GetByCustomerIdAsync(registrationForm.CustomerId.ToString());

            //CountryPhoneCodeId is required for verified customers so we MUST have a value
            registrationForm.CountryPhoneCodeId = customerProfile.Profile.CountryPhoneCodeId.Value;
            registrationForm.PhoneNumber = customerProfile.Profile.ShortPhoneNumber;

            await UpdateAgentProfileAsync(registrationForm);

            var encryptedImages = EncryptImages(registrationForm.CustomerId, registrationForm.Images);

            var isDemoEmail = _settingsService.IsDemoEmail(customerProfile.Profile.Email);
            
            if (agent == null)
            {
                agent = new Agent(registrationForm.CustomerId)
                {
                    Status = isDemoEmail ? AgentStatus.ApprovedAgent : AgentStatus.NotAgent
                };
                await _agentRepository.InsertAsync(agent, encryptedImages);
            }
            else
                await _imageRepository.UpdateAsync(encryptedImages);

            await CreateSalesforceAccountAsync(registrationForm.CustomerId, registrationForm.Note,
                registrationForm.Images);

            _log.Info("Customer registered as an agent.", context: $"customerId: {registrationForm.CustomerId};");
        }

        public async Task<CustomerRequirements> ValidateAsync(Guid customerId)
        {
            var agent = await _agentRepository.GetByCustomerIdAsync(customerId);

            var status = agent?.Status ?? AgentStatus.NotAgent;

            var hasVerifiedEmail = await ValidateEmailAsync(customerId);

            var hasEnoughTokens = await ValidateNumberOfTokensAsync(customerId);

            return new CustomerRequirements
            {
                CustomerId = customerId,
                HasVerifiedEmail = hasVerifiedEmail,
                HasEnoughTokens = hasEnoughTokens,
                IsEligible = hasEnoughTokens && hasVerifiedEmail && status == AgentStatus.NotAgent,
                Status = status
            };
        }

        private async Task<bool> ValidateNumberOfTokensAsync(Guid customerId)
        {
            var pbfResponse = await _pbfClient.CustomersApi.GetBalanceAsync(customerId);

            return pbfResponse.Total >= await _requirementsService.GetNumberOfTokensAsync();
        }

        private async Task<bool> ValidateEmailAsync(Guid customerId)
        {
            var response = await _customerProfileClient.CustomerProfiles.GetByCustomerIdAsync(customerId.ToString());

            if (response.ErrorCode == CustomerProfileErrorCodes.CustomerProfileDoesNotExist)
                throw new CustomerProfileNotFoundException();

            var customerProfile = response.Profile;

            return customerProfile.IsEmailVerified;
        }

        private Task UpdateAgentProfileAsync(RegistrationForm registrationForm)
            => _customerProfileClient.CustomerProfiles.UpdateAsync(new CustomerProfileUpdateRequestModel
            {
                CustomerId = registrationForm.CustomerId.ToString(),
                FirstName = registrationForm.FirstName,
                LastName = registrationForm.LastName,
                PhoneNumber = registrationForm.PhoneNumber,
                CountryPhoneCodeId = registrationForm.CountryPhoneCodeId,
                CountryOfResidenceId = registrationForm.CountryOfResidenceId
            });

        private async Task CreateSalesforceAccountAsync(Guid customerId, string note, IEnumerable<Image> images)
        {
            var registrationResponse = await _tokenPropertyIntegrationClient.Api
                .RegisterConnectorAsync(new ConnectorRegisterRequestModel
                {
                    CustomerId = customerId.ToString(),
                    Note = note,
                    Images = images.Select(o => new AgentImage
                    {
                        ImageName = o.Name,
                        ImageBase64 = o.Content,
                        DocumentType = Enum.Parse<ImageDocumentType>(o.DocumentType.ToString())
                    }).ToList()
                });

            switch (registrationResponse.Status)
            {
                case ConnectorRegisterStatus.Ok:
                    await _agentRepository.UpdateStatusAsync(customerId, registrationResponse.ConnectorSalesforceId,
                        AgentStatus.ApprovedAgent);

                    var approvedEvent = new PushNotificationEvent
                    {
                        CustomerId = customerId.ToString(),
                        MessageTemplateId = _agentApprovedPushTemplateId,
                        Source = _componentSourceName,
                    };
                    await _pushNotificationPublisher.PublishAsync(approvedEvent);

                    break;

                case ConnectorRegisterStatus.AlreadyExists:
                    await _agentRepository.UpdateStatusAsync(customerId, AgentStatus.Rejected);

                    var rejectedEvent = new PushNotificationEvent
                    {
                        CustomerId = customerId.ToString(),
                        MessageTemplateId = _agentRejectedPushTemplateId,
                        Source = _componentSourceName,
                    };
                    await _pushNotificationPublisher.PublishAsync(rejectedEvent);

                    var evt = new EmailMessageEvent
                    {
                        CustomerId = customerId.ToString(),
                        SubjectTemplateId = _agentRejectedEmailSubjectTemplateId,
                        MessageTemplateId = _agentRejectedEmailTemplateId,
                        Source = _componentSourceName,
                    };
                    await _emailNotificationPublisher.PublishAsync(evt);

                    throw new SalesforceAccountAlreadyExistsException();

                case ConnectorRegisterStatus.ImageUploadError:
                    throw new SalesforceImageUploadFailException();

                case ConnectorRegisterStatus.ConnectorRegistrationError:
                    throw new SalesforceAccountRegistrationFailException();

                default:
                    throw new InvalidOperationException("Received unknown status while creating salesforce account.");
            }
        }

        private static IReadOnlyList<EncryptedImage> EncryptImages(Guid customerId, IEnumerable<Image> images)
            => images
                .Select(o => new EncryptedImage
                {
                    CustomerId = customerId,
                    DocumentType = o.DocumentType,
                    Name = o.Name,
                    Hash = o.Content.ToSha256Hash()
                })
                .ToList();
    }
}
