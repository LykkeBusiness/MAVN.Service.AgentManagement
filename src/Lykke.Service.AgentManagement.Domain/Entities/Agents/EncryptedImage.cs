using System;

namespace Lykke.Service.AgentManagement.Domain.Entities.Agents
{
    /// <summary>
    /// Represents an agent encrypted image.
    /// </summary>
    public class EncryptedImage
    {
        public EncryptedImage()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        /// <summary>
        /// The customer identifier.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// The document type of the image.
        /// </summary>
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// The name of the image.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The image hash code.
        /// </summary>
        public string Hash { get; set; }
    }
}
