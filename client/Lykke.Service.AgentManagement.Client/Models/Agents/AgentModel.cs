using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.AgentManagement.Client.Models.Agents
{
    /// <summary>
    /// Represents an agent information.
    /// </summary>
    [PublicAPI]
    public class AgentModel
    {
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
        [JsonConverter(typeof(StringEnumConverter))]
        public AgentStatus Status { get; set; }
    }
}
