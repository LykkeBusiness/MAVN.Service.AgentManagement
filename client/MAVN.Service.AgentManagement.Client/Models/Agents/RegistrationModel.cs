using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace MAVN.Service.AgentManagement.Client.Models.Agents
{
    /// <summary>
    /// Represents an agent registration information for KYA process.
    /// </summary>
    [PublicAPI]
    public class RegistrationModel
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
        public IReadOnlyList<ImageModel> Images { get; set; }
    }
}
