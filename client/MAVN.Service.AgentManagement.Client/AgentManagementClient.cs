using Lykke.HttpClientGenerator;
using MAVN.Service.AgentManagement.Client.Api;

namespace MAVN.Service.AgentManagement.Client
{
    /// <inheritdoc/>
    public class AgentManagementClient : IAgentManagementClient
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AgentManagementClient"/> with <param name="httpClientGenerator"></param>.
        /// </summary> 
        public AgentManagementClient(IHttpClientGenerator httpClientGenerator)
        {
            Agents = httpClientGenerator.Generate<IAgentsApi>();
            Requirements = httpClientGenerator.Generate<IRequirementsApi>();
        }

        /// <inheritdoc />
        public IAgentsApi Agents { get; set; }

        /// <inheritdoc />
        public IRequirementsApi Requirements { get; set; }
    }
}
