using System;
using System.Collections.Generic;

namespace MAVN.Service.AgentManagement.Domain.Entities.Registration
{
    /// <summary>
    /// Represents an agent registration information for KYA process.
    /// </summary>
    public class RegistrationForm
    {
        /// <summary>
        /// The customer identifier.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// The customer first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The customer last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The customer phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The phone country dialing code identifier.
        /// </summary>
        public int CountryPhoneCodeId { get; set; }

        /// <summary>
        /// The country of residence identifier.
        /// </summary>
        public int CountryOfResidenceId { get; set; }

        /// <summary>
        /// The note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// A collection of agent photos required for KYA.
        /// </summary>
        public IReadOnlyList<Image> Images { get; set; }
    }
}
