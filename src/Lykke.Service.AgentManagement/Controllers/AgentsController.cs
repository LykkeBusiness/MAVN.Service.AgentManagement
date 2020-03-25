using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.AgentManagement.Client.Api;
using Lykke.Service.AgentManagement.Client.Models;
using Lykke.Service.AgentManagement.Client.Models.Agents;
using Lykke.Service.AgentManagement.Domain.Entities.Registration;
using Lykke.Service.AgentManagement.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Lykke.Service.AgentManagement.Domain.Exceptions;
using Lykke.Service.Dictionaries.Client;

namespace Lykke.Service.AgentManagement.Controllers
{
    [ApiController]
    [Route("api/agents")]
    public class AgentsController : ControllerBase, IAgentsApi
    {
        private readonly IAgentService _agentService;
        private readonly IDictionariesClient _dictionariesClient;
        private readonly ILog _log;
        private readonly IMapper _mapper;

        public AgentsController(
            IAgentService agentService,
            IDictionariesClient dictionariesClient,
            ILogFactory logFactory,
            IMapper mapper)
        {
            _agentService = agentService;
            _dictionariesClient = dictionariesClient;
            _log = logFactory.CreateLog(this);
            _mapper = mapper;
        }

        /// <summary>
        /// Returns an agent information by customer id.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>
        /// 200 - The customer agent status and other details.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(AgentModel), (int) HttpStatusCode.OK)]
        public async Task<AgentModel> GetByCustomerIdAsync(Guid customerId)
        {
            var agent = await _agentService.GetByCustomerIdAsync(customerId);

            return _mapper.Map<AgentModel>(agent);
        }

        /// <summary>
        /// Returns an agents information by customer ids.
        /// </summary>
        /// <param name="customerIds">The customers identifiers.</param>
        /// <returns>
        /// 200 - The customers agents statuses and other details.
        /// </returns>
        [HttpPost("list")]
        [ProducesResponseType(typeof(IReadOnlyCollection<AgentModel>), (int) HttpStatusCode.OK)]
        public async Task<IReadOnlyCollection<AgentModel>> GetByCustomerIdsAsync([FromBody] Guid[] customerIds)
        {
            var agent = await _agentService.GetByCustomerIdsAsync(customerIds);

            return _mapper.Map<IReadOnlyCollection<AgentModel>>(agent);
        }

        /// <summary>
        /// Registers a customer as an agent.
        /// </summary>
        /// <param name="model">The agent KYA data.</param>
        /// <remarks>
        /// Used to register new agent.
        /// 
        /// Error codes:
        /// - **AgentAlreadyApproved**
        /// - **EmailNotVerified**
        /// - **NotEnoughTokens**
        /// - **CustomerProfileDoesNotExist**
        /// - **CountryPhoneCodeDoesNotExist**
        /// - **CountryOfResidenceDoesNotExist**
        /// - **AccountAlreadyExists**
        /// - **ImageUploadFail**
        /// - **AccountRegistrationFail**
        /// </remarks>
        /// <returns>
        /// 204 - Customer successfully registered as an agent.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(RegistrationResultModel), (int) HttpStatusCode.NoContent)]
        public async Task<RegistrationResultModel> RegisterAsync([FromBody] RegistrationModel model)
        {
            try
            {
                var countryOfResidence = await _dictionariesClient.Salesforce
                    .GetCountryOfResidenceByIdAsync(model.CountryOfResidenceId);

                if (countryOfResidence == null)
                    return new RegistrationResultModel(AgentManagementErrorCode.CountryOfResidenceDoesNotExist);

                var registrationForm = _mapper.Map<RegistrationForm>(model);

                await _agentService.RegisterAsync(registrationForm);
            }
            catch (AlreadyApprovedException exception)
            {
                _log.Info(exception.Message, context: $"customerId: {model.CustomerId}");
                return new RegistrationResultModel(AgentManagementErrorCode.AgentAlreadyApproved);
            }
            catch (EmailNotVerifiedException exception)
            {
                _log.Info(exception.Message, context: $"customerId: {model.CustomerId}");
                return new RegistrationResultModel(AgentManagementErrorCode.EmailNotVerified);
            }
            catch (NotEnoughTokensException exception)
            {
                _log.Info(exception.Message, context: $"customerId: {model.CustomerId}");
                return new RegistrationResultModel(AgentManagementErrorCode.NotEnoughTokens);
            }
            catch (CustomerProfileNotFoundException exception)
            {
                _log.Warning(exception.Message, context: $"customerId: {model.CustomerId}");
                return new RegistrationResultModel(AgentManagementErrorCode.CustomerProfileDoesNotExist);
            }
            catch (SalesforceAccountAlreadyExistsException exception)
            {
                _log.Warning(exception.Message, context: $"customerId: {model.CustomerId}");
                return new RegistrationResultModel(AgentManagementErrorCode.AccountAlreadyExists);
            }
            catch (SalesforceImageUploadFailException exception)
            {
                _log.Warning(exception.Message, context: $"customerId: {model.CustomerId}");
                return new RegistrationResultModel (AgentManagementErrorCode.ImageUploadFail);
            }
            catch (SalesforceAccountRegistrationFailException exception)
            {
                _log.Warning(exception.Message, context: $"customerId: {model.CustomerId}");
                return new RegistrationResultModel(AgentManagementErrorCode.AccountRegistrationFail);
            }

            return new RegistrationResultModel();
        }
    }
}
