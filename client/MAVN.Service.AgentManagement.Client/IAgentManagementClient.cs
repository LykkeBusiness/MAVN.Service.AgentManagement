using JetBrains.Annotations;
using MAVN.Service.AgentManagement.Client.Api;

namespace MAVN.Service.AgentManagement.Client
{
    /// <summary>
    /// Agent management API service client.
    /// </summary>
    [PublicAPI]
    public interface IAgentManagementClient
    {
        /// <summary>
        /// Agents API.
        /// </summary>
        IAgentsApi Agents { get; set; }

        /// <summary>
        /// Requirements API.
        /// </summary>
        IRequirementsApi Requirements { get; set; }
    }
}
