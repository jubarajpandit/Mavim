using Mavim.Libraries.Authorization.Interfaces;

namespace Mavim.Libraries.Authorization.Models
{
    public class TopicAuthorization : IAuthorization
    {
        public bool Readonly { get; set; }
        public bool IsAdmin { get; set; }
    }
}
