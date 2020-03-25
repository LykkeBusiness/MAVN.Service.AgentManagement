using Falcon.Numerics;
using JetBrains.Annotations;

namespace Lykke.Service.AgentManagement.Client.Models.Requirements
{
    /// <summary>
    /// Represents an update model on customer tokens requirements to become an agent.
    /// </summary>
    [PublicAPI]
    public class UpdateTokensRequirementModel
    {
        /// <summary>
        /// The new number of tokens.
        /// </summary>
        public Money18? Amount { get; set; }
    }
}
