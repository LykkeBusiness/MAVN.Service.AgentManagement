using JetBrains.Annotations;

namespace MAVN.Service.AgentManagement.Client.Models.Agents
{
    /// <summary>
    /// Represents a result of agent registration.
    /// </summary>
    [PublicAPI]
    public class RegistrationResultModel
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RegistrationResultModel"/>.
        /// </summary>
        public RegistrationResultModel()
        {
            ErrorCode = AgentManagementErrorCode.None;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RegistrationResultModel"/> with error code.
        /// </summary>
        public RegistrationResultModel(AgentManagementErrorCode agentManagementErrorCode)
        {
            ErrorCode = agentManagementErrorCode;
        }

        /// <summary>
        /// Specifies an error code of agent registration code.
        /// </summary>
        public AgentManagementErrorCode ErrorCode { get; set; }
    }
}
