using System;
using System.IdentityModel.Tokens.Jwt;

namespace Mavim.Libraries.Authorization.Interfaces
{
    public interface IJwtSecurityToken
    {
        JwtSecurityToken Token { get; set; }
        string RawData { get; set; }
        Guid TenantId { get; set; }
        Guid AppId { get; set; }
        Guid UserID { get; set; }
        string Email { get; set; }
        string Name { get; set; }
    }
}
