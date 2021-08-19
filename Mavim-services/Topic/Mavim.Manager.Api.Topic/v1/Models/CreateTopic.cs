using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Topic.v1.Models
{
    /// <summary>
    /// CreateTopic
    /// </summary>
    public class CreateTopic
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the type
        /// </summary>
        [Required]
        public string Type { get; set; }
        /// <summary>
        /// Gets or sets the icon
        /// </summary>
        [Required]
        public string Icon { get; set; }
    }
}
