using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.AgentManagement.Client.Models.Agents;
using Refit;

namespace Lykke.Service.AgentManagement.Client.Api
{
    /// <summary>
    /// Provides methods to work with agents.
    /// </summary>
    [PublicAPI]
    public interface IAgentsApi
    {
        /// <summary>
        /// Returns an agent information by customer id.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>The customer agent status and other details.</returns>
        [Get("/api/agents")]
        Task<AgentModel> GetByCustomerIdAsync(Guid customerId);

        /// <summary>
        /// Returns an agents information by customer ids.
        /// </summary>
        /// <param name="customerIds">The customers identifiers.</param>
        /// <returns>The customers agents statuses and other details.</returns>
        [Post("/api/agents/list")]
        Task<IReadOnlyCollection<AgentModel>> GetByCustomerIdsAsync([Body] Guid[] customerIds);

        /// <summary>
        /// Registers a customer as an agent.
        /// </summary>
        /// <param name="model">The agent KYA data.</param>
        /// <returns>The result of customer registration.</returns>
        [Post("/api/agents")]
        Task<RegistrationResultModel> RegisterAsync([Body] RegistrationModel model);
    }
}
