namespace Lykke.Service.AgentManagement.Domain.Entities.Agents
{
    /// <summary>
    /// Specifies status of agent.
    /// </summary>
    public enum AgentStatus
    {
        /// <summary>
        /// Unspecified status.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that the customer is not an agent.
        /// </summary>
        NotAgent,

        /// <summary>
        /// Indicates that the customer registration as an agent was rejected.
        /// </summary>
        Rejected,
        
        /// <summary>
        /// Indicates that the customer registered as an agent and successfully complete KYA.
        /// </summary>
        ApprovedAgent
    }
}
