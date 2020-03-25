using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Falcon.Numerics;
using Lykke.Logs;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.Service.AgentManagement.Domain.Entities;
using Lykke.Service.AgentManagement.Domain.Entities.Agents;
using Lykke.Service.AgentManagement.Domain.Entities.Registration;
using Lykke.Service.AgentManagement.Domain.Exceptions;
using Lykke.Service.AgentManagement.Domain.Repositories;
using Lykke.Service.AgentManagement.Domain.Services;
using Lykke.Service.CustomerProfile.Client;
using Lykke.Service.CustomerProfile.Client.Models.Enums;
using Lykke.Service.CustomerProfile.Client.Models.Requests;
using Lykke.Service.CustomerProfile.Client.Models.Responses;
using Lykke.Service.MAVNPropertyIntegration.Client;
using Lykke.Service.MAVNPropertyIntegration.Client.Enums;
using Lykke.Service.MAVNPropertyIntegration.Client.Models.Requests;
using Lykke.Service.MAVNPropertyIntegration.Client.Models.Responses;
using Lykke.Service.NotificationSystem.SubscriberContract;
using Lykke.Service.PrivateBlockchainFacade.Client;
using Lykke.Service.PrivateBlockchainFacade.Client.Models;
using Moq;
using Xunit;

namespace Lykke.Service.AgentManagement.DomainServices.Tests
{
    public class AgentServiceTests
    {
        private const string TokenName = "TOKEN";
        private static readonly Money18 RequiredNumberOfTokens = Money18.Create(100);

        private readonly Mock<IRequirementsService> _requirementsServiceMock =
            new Mock<IRequirementsService>();

        private readonly Mock<ISettingsService> _settingsServiceMock =
            new Mock<ISettingsService>();

        private readonly Mock<IAgentRepository> _agentRepositoryMock =
            new Mock<IAgentRepository>();

        private readonly Mock<IImageRepository> _imageRepositoryMock =
            new Mock<IImageRepository>();

        private readonly Mock<ICustomerProfileClient> _customerProfileClientMock =
            new Mock<ICustomerProfileClient>();

        private readonly Mock<IPrivateBlockchainFacadeClient> _pbfClientMock =
            new Mock<IPrivateBlockchainFacadeClient>();

        private readonly Mock<IRabbitPublisher<PushNotificationEvent>> _pushNotificationPublisherMock =
            new Mock<IRabbitPublisher<PushNotificationEvent>>();

        private readonly Mock<IRabbitPublisher<EmailMessageEvent>> _emailNotificationPublisherMock =
            new Mock<IRabbitPublisher<EmailMessageEvent>>();

        private readonly Mock<IMAVNPropertyIntegrationClient> _tokenPropertyIntegrationClientMock =
            new Mock<IMAVNPropertyIntegrationClient>();
        
        private readonly IAgentService _service;

        private readonly CustomerProfileResponse _customerProfileResponse = new CustomerProfileResponse
        {
            Profile = new CustomerProfile.Client.Models.Responses.CustomerProfile
            {
                CustomerId = Guid.NewGuid().ToString(),
                Email = "test@test.com",
                IsEmailVerified = true,
                Registered = DateTime.UtcNow.AddDays(-10),
                ShortPhoneNumber = "123456",
                CountryPhoneCodeId = 1
            },
            ErrorCode = CustomerProfileErrorCodes.None
        };

        private readonly ConnectorRegisterResponseModel _connectorRegisterResponse = new ConnectorRegisterResponseModel
        {
            Status = ConnectorRegisterStatus.Ok, ConnectorSalesforceId = "sf_id"
        };

        private readonly CustomerBalanceResponseModel _pbfResponse = new CustomerBalanceResponseModel {Total = 1000};

        private readonly RegistrationForm _registrationForm = Mock.Of<RegistrationForm>();

        public AgentServiceTests()
        {
            _registrationForm.Images = new List<Image>
            {
                new Image {DocumentType = DocumentType.Passport, Name = "passport.jpg", Content = "aGVsbG8="}
            };

            _settingsServiceMock.Setup(o => o.GetTokenName())
                .Returns(TokenName);

            _requirementsServiceMock.Setup(o => o.GetNumberOfTokensAsync())
                .ReturnsAsync(RequiredNumberOfTokens);

            _customerProfileClientMock
                .Setup(o => o.CustomerProfiles.GetByCustomerIdAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(_customerProfileResponse));

            _pbfClientMock.Setup(o => o.CustomersApi.GetBalanceAsync(It.IsAny<Guid>()))
                .Returns((Guid customerId) => Task.FromResult(_pbfResponse));

            _tokenPropertyIntegrationClientMock
                .Setup(o => o.Api.RegisterConnectorAsync(It.IsAny<ConnectorRegisterRequestModel>()))
                .ReturnsAsync((ConnectorRegisterRequestModel model) => _connectorRegisterResponse);

            _service = new AgentService(
                "approvedTemplatedId",
                "rejectedTemplateId",
                "agentRejectedEmailSubjectTemplateId",
                "agentRejectedEmailTemplateId",
                _requirementsServiceMock.Object,
                _agentRepositoryMock.Object,
                _imageRepositoryMock.Object,
                _customerProfileClientMock.Object,
                _pbfClientMock.Object,
                _pushNotificationPublisherMock.Object,
                _emailNotificationPublisherMock.Object,
                _tokenPropertyIntegrationClientMock.Object,
                _settingsServiceMock.Object,
                EmptyLogFactory.Instance);
        }

        [Fact]
        public async Task Create_New_Agent_While_Registering_If_Not_Exists()
        {
            // act

            await _service.RegisterAsync(_registrationForm);

            // assert

            _agentRepositoryMock.Verify(
                o => o.InsertAsync(It.IsAny<Agent>(), It.IsAny<IReadOnlyList<EncryptedImage>>()),
                Times.Once);
        }

        [Fact]
        public async Task Update_Customer_Profile_While_Registering_An_Agent()
        {
            // act

            await _service.RegisterAsync(_registrationForm);

            // assert

            _customerProfileClientMock.Verify(
                o => o.CustomerProfiles.UpdateAsync(It.IsAny<CustomerProfileUpdateRequestModel>()),
                Times.Once);
        }

        [Fact]
        public async Task Do_Not_Register_If_Agent_Already_Approved()
        {
            // arrange

            var agent = new Agent(Guid.NewGuid()) {Status = AgentStatus.ApprovedAgent};

            _agentRepositoryMock.Setup(o => o.GetByCustomerIdAsync(It.IsAny<Guid>()))
                .Returns((Guid customerId) => Task.FromResult(agent));

            // act

            var task = _service.RegisterAsync(_registrationForm);

            // assert

            await Assert.ThrowsAsync<AlreadyApprovedException>(async () => await task);
        }

        [Fact]
        public async Task Do_Not_Register_If_Agent_Rejected()
        {
            // arrange

            var agent = new Agent(Guid.NewGuid()) {Status = AgentStatus.Rejected};

            _agentRepositoryMock.Setup(o => o.GetByCustomerIdAsync(It.IsAny<Guid>()))
                .Returns((Guid customerId) => Task.FromResult(agent));

            // act

            var task = _service.RegisterAsync(_registrationForm);

            // assert

            await Assert.ThrowsAsync<SalesforceAccountAlreadyExistsException>(async () => await task);
        }

        [Fact]
        public async Task Do_Not_Register_If_Customer_Profile_Does_Not_Exist()
        {
            // arrange

            _customerProfileResponse.ErrorCode = CustomerProfileErrorCodes.CustomerProfileDoesNotExist;

            // act

            var task = _service.RegisterAsync(_registrationForm);

            // assert

            await Assert.ThrowsAsync<CustomerProfileNotFoundException>(async () => await task);
        }

        [Fact]
        public async Task Do_Not_Register_If_Customer_Email_Not_Verified()
        {
            // arrange

            _customerProfileResponse.Profile.IsEmailVerified = false;

            // act

            var task = _service.RegisterAsync(_registrationForm);

            // assert

            await Assert.ThrowsAsync<EmailNotVerifiedException>(async () => await task);
        }

        [Fact]
        public async Task Do_Not_Register_If_Customer_Has_No_Enough_Tokens()
        {
            // arrange

            _pbfResponse.Total = RequiredNumberOfTokens - 1;

            // act

            var task = _service.RegisterAsync(_registrationForm);

            // assert

            await Assert.ThrowsAsync<NotEnoughTokensException>(async () => await task);
        }

        [Fact]
        public async Task Send_Push_Notification_If_Registration_Succeeded()
        {
            // act

            await _service.RegisterAsync(_registrationForm);

            // assert

            _pushNotificationPublisherMock.Verify(o => o.PublishAsync(It.IsAny<PushNotificationEvent>()), Times.Once);
        }

        [Fact]
        public async Task Throw_Error_If_Agent_Already_Registered_In_Salesforce()
        {
            // arrange

            _connectorRegisterResponse.Status = ConnectorRegisterStatus.AlreadyExists;
            
            // act

            var task = _service.RegisterAsync(_registrationForm);

            // assert

            await Assert.ThrowsAsync<SalesforceAccountAlreadyExistsException>(async () => await task);
        }
        
        [Fact]
        public async Task Throw_Error_If_An_Error_Occurred_While_Uploading_Images_To_The_Salesforce()
        {
            // arrange

            _connectorRegisterResponse.Status = ConnectorRegisterStatus.ImageUploadError;
            
            // act

            var task = _service.RegisterAsync(_registrationForm);

            // assert

            await Assert.ThrowsAsync<SalesforceImageUploadFailException>(async () => await task);
        }

        [Fact]
        public async Task Throw_Error_If_An_Error_Occurred_While_Calling_Salesforce()
        {
            // arrange

            _connectorRegisterResponse.Status = ConnectorRegisterStatus.ConnectorRegistrationError;

            // act

            var task = _service.RegisterAsync(_registrationForm);

            // assert

            await Assert.ThrowsAsync<SalesforceAccountRegistrationFailException>(async () => await task);
        }

        [Fact]
        public async Task Send_Push_Notification_If_Registration_Failed()
        {
            // arrange

            _connectorRegisterResponse.Status = ConnectorRegisterStatus.AlreadyExists;

            // act

            try
            {
                await _service.RegisterAsync(_registrationForm);
            }
            catch
            {
                // ignored
            }

            // assert

            _pushNotificationPublisherMock.Verify(o => o.PublishAsync(It.IsAny<PushNotificationEvent>()), Times.Once);
        }

        [Fact]
        public async Task Send_Email_Notification_If_Registration_Failed()
        {
            // arrange

            _connectorRegisterResponse.Status = ConnectorRegisterStatus.AlreadyExists;

            // act

            try
            {
                await _service.RegisterAsync(_registrationForm);
            }
            catch
            {
                // ignored
            }

            // assert

            _emailNotificationPublisherMock.Verify(o => o.PublishAsync(It.IsAny<EmailMessageEvent>()), Times.Once);
        }

        [Fact]
        public async Task Return_Ineligible_If_Email_Not_Verified()
        {
            // arrange

            _customerProfileResponse.Profile.IsEmailVerified = false;

            // act

            var customerRequirements = await _service.ValidateAsync(Guid.NewGuid());

            // assert

            Assert.False(customerRequirements.IsEligible);
        }

        [Fact]
        public async Task Return_Ineligible_If_No_Enough_Tokens()
        {
            // arrange

            _pbfResponse.Total = RequiredNumberOfTokens - 1;

            // act

            var customerRequirements = await _service.ValidateAsync(Guid.NewGuid());

            // assert

            Assert.False(customerRequirements.IsEligible);
        }
    }
}
