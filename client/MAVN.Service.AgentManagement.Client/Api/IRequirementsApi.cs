using System;
using System.Threading.Tasks;
using Falcon.Numerics;
using JetBrains.Annotations;
using MAVN.Service.AgentManagement.Client.Models.Requirements;
using Refit;

namespace MAVN.Service.AgentManagement.Client.Api
{
    /// <summary>
    /// Provides methods to work
    /// </summary>
    [PublicAPI]
    public interface IRequirementsApi
    {
        /// <summary>
        /// Returns the customer requirements.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>The customer acceptance criteria for KYA process.</returns>
        [Get("/api/requirements")]
        Task<CustomerRequirementsModel> GetByCustomerIdAsync(Guid customerId);

        /// <summary>
        /// Returns a customer tokens requirements.
        /// </summary>
        /// <returns>The customer tokens requirements to become an agent.</returns>
        [Get("/api/requirements/tokens")]
        Task<TokensRequirementModel> GetTokensRequirementsAsync();

        /// <summary>
        /// Updates tokens requirements.
        /// </summary>
        /// <returns>The token requirement has been updated.</returns>
        [Put("/api/requirements/tokens")]
        Task UpdateTokensRequirementAsync(UpdateTokensRequirementModel requirement);
    }
}
