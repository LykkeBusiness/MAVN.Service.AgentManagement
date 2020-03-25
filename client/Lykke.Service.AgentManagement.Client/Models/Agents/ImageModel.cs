using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.AgentManagement.Client.Models.Agents
{
    /// <summary>
    /// Represents an image details.
    /// </summary>
    [PublicAPI]
    public class ImageModel
    {
        /// <summary>
        /// The document type of the image.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// The name of the image.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The image encoded in base64.
        /// </summary>
        public string Content { get; set; }
    }
}
