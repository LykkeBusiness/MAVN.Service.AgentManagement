using System;
using MAVN.Service.AgentManagement.Domain.Entities.Agents;

namespace MAVN.Service.AgentManagement.Domain.Entities.Requirements
{
    /// <summary>
    /// Represents a customer acceptance criteria for KYA process.
    /// </summary>
    public class CustomerRequirements
    {
        /// <summary>
        /// The customer identifier.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// If <c>true</c> all requirements completed and the customer could become an agent, otherwise <c>false</c>.
        /// </summary>
        public bool IsEligible { get; set; }

        /// <summary>
        /// Indicated that customer has enough tokens to become agent. 
        /// </summary>
        public bool HasEnoughTokens { get; set; }

        /// <summary>
        /// Indicated that customer email verified. 
        /// </summary>
        public bool HasVerifiedEmail { get; set; }

        /// <summary>
        /// The current status of an agent.
        /// </summary>
        public AgentStatus Status { get; set; }
    }
}
