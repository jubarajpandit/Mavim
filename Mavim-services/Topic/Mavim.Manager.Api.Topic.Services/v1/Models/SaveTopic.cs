using Mavim.Manager.Api.Topic.Services.Interfaces.v1;

namespace Mavim.Manager.Api.Topic.Services.v1.Models
{
    public class SaveTopic : ISaveTopic
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Name { get; set; }
    }
}
