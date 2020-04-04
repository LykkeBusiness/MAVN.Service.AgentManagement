namespace MAVN.Service.AgentManagement.Domain.Entities.Registration
{
    /// <summary>
    /// Represents an agent image.
    /// </summary>
    public class Image
    {
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
        public string Content { get; set; }
    }
}
