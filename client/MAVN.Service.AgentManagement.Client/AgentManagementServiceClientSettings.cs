using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.AgentManagement.Client 
{
    /// <summary>
    /// AgentManagement client settings.
    /// </summary>
    [PublicAPI]
    public class AgentManagementServiceClientSettings 
    {
        /// <summary>
        /// The service url.
        /// </summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
