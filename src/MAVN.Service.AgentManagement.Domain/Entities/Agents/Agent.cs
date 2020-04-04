using System;

namespace MAVN.Service.AgentManagement.Domain.Entities.Agents
{
    /// <summary>
    /// Represents an agent information.
    /// </summary>
    public class Agent
    {
        public Agent()
        {
        }

        public Agent(Guid customerId)
        {
            CustomerId = customerId;
            Status = AgentStatus.NotAgent;
        }

        /// <summary>
        /// The customer identifier.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// The customer identifier in salesforce.
        /// </summary>
        public string SalesforceId { get; set; }

        /// <summary>
        /// Indicates the agent status.
        /// </summary>
        public AgentStatus Status { get; set; }
    }
}
