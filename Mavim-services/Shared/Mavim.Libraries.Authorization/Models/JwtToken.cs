using Mavim.Libraries.Authorization.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Mavim.Libraries.Authorization.Models
{
    public class JwtToken : IJwtSecurityToken
    {
        public JwtSecurityToken Token { get; set; }
        public string RawData { get; set; }
        public Guid TenantId { get; set; }
        public Guid AppId { get; set; }
        public Guid UserID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
