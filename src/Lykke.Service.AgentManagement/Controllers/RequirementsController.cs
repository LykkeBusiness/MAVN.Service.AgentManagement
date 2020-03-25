using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Falcon.Numerics;
using Lykke.Service.AgentManagement.Client.Api;
using Lykke.Service.AgentManagement.Client.Models.Requirements;
using Lykke.Service.AgentManagement.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.AgentManagement.Controllers
{
    [ApiController]
    [Route("api/requirements")]
    public class RequirementsController : ControllerBase, IRequirementsApi
    {
        private readonly IAgentService _agentService;
        private readonly IRequirementsService _requirementsService;
        private readonly IMapper _mapper;

        public RequirementsController(
            IAgentService agentService,
            IRequirementsService requirementsService,
            IMapper mapper)
        {
            _agentService = agentService;
            _requirementsService = requirementsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns the customer requirements.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>
        /// 200 - the customer acceptance criteria for KYA process.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(CustomerRequirementsModel), (int) HttpStatusCode.OK)]
        public async Task<CustomerRequirementsModel> GetByCustomerIdAsync(Guid customerId)
        {
            var customerRequirements = await _agentService.ValidateAsync(customerId);

            return _mapper.Map<CustomerRequirementsModel>(customerRequirements);
        }

        /// <summary>
        /// Returns a customer tokens requirements.
        /// </summary>
        /// <returns>
        /// 200 - the customer tokens requirements to become an agent.
        /// </returns>
        [HttpGet("tokens")]
        [ProducesResponseType(typeof(TokensRequirementModel), (int) HttpStatusCode.OK)]
        public async Task<TokensRequirementModel> GetTokensRequirementsAsync()
        {
            var numberOfTokens = await _requirementsService.GetNumberOfTokensAsync();

            return new TokensRequirementModel {RequiredNumberOfTokens = numberOfTokens};
        }

        /// <summary>
        /// Updates tokens requirements.
        /// </summary>
        /// <returns>
        /// 200 - the token requirement has been updated.
        /// </returns>
        [HttpPut("tokens")]
        [ProducesResponseType(typeof(TokensRequirementModel), (int)HttpStatusCode.OK)]
        public async Task UpdateTokensRequirementAsync(UpdateTokensRequirementModel requirement)
        {
            await _requirementsService.UpdateNumberOfTokensAsync(requirement.Amount.Value);
        }
    }
}
