using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Topic.v1.Models
{
    /// <summary>
    /// SaveTopic
    /// </summary>
    public class SaveTopic : ISaveTopic
    {
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [MaxLength(2000, ErrorMessage = "Name cannot exceed 2000 characters.")]
        public string Name { get; set; }
    }
}
