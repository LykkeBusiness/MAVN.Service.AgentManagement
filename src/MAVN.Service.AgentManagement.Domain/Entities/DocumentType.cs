namespace MAVN.Service.AgentManagement.Domain.Entities
{
    /// <summary>
    /// Specifies the document types.
    /// </summary>
    public enum DocumentType
    {
        /// <summary>
        /// Unspecified document type.
        /// </summary>
        None,

        /// <summary>
        /// Indicates passport.
        /// </summary>
        Passport,

        /// <summary>
        /// Indicates driver license front side.
        /// </summary>
        DriverLicenseFront,

        /// <summary>
        /// Indicates driver license back side.
        /// </summary>
        DriverLicenseBack,

        /// <summary>
        /// Indicates national ID front side.
        /// </summary>
        NationalIdFront,

        /// <summary>
        /// Indicates national ID back side.
        /// </summary>
        NationalIdBack
    }
}
