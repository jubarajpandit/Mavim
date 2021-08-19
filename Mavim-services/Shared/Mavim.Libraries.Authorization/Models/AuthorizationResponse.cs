using System;

namespace Mavim.Libraries.Authorization.Models
{
    public class TopicAuthorizationResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid TenantId { get; set; }
        public Role Role { get; set; }
    }
}
