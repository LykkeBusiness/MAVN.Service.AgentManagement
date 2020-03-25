using Falcon.Numerics;
using JetBrains.Annotations;

namespace Lykke.Service.AgentManagement.Client.Models.Requirements
{
    /// <summary>
    /// Represents a customer tokens requirements to become an agent.
    /// </summary>
    [PublicAPI]
    public class TokensRequirementModel
    {
        /// <summary>
        /// The required number of tokens.
        /// </summary>
        public Money18 RequiredNumberOfTokens { get; set; }
    }
}
