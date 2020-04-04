using JetBrains.Annotations;

namespace MAVN.Service.AgentManagement.Client.Models
{
    /// <summary>
    /// Specifies agent management service error codes.
    /// </summary>
    [PublicAPI]
    public enum AgentManagementErrorCode
    {
        /// <summary>
        /// Unspecified error.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that the customer does not exist.
        /// </summary>
        CustomerProfileDoesNotExist,

        /// <summary>
        /// Indicates that the email is not verified.
        /// </summary>
        EmailNotVerified,

        /// <summary>
        /// Indicates that the customer has not enough tokens.
        /// </summary>
        NotEnoughTokens,

        /// <summary>
        /// Indicates that the customer already registered and approved as an agent.
        /// </summary>
        AgentAlreadyApproved,

        /// <summary>
        /// Indicates that the country phone code does not exist.
        /// </summary>
        CountryPhoneCodeDoesNotExist,

        /// <summary>
        /// Indicates that the country of residence does not exist.
        /// </summary>
        CountryOfResidenceDoesNotExist,

        /// <summary>
        /// Indicates that the customer already register as an agent in Salesforce.
        /// </summary>
        AccountAlreadyExists,

        /// <summary>
        /// Indicates that the an error occurred while uploading images to Salesforce.
        /// </summary>
        ImageUploadFail,

        /// <summary>
        /// Indicates that the an error occurred while registering an agent account in Salesforce.
        /// </summary>
        AccountRegistrationFail
    }
}
