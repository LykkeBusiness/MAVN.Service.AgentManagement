using JetBrains.Annotations;
using Lykke.Service.AgentManagement.Client.Api;

namespace Lykke.Service.AgentManagement.Client
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
