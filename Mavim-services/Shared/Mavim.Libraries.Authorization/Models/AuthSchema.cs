using Microsoft.IdentityModel.Tokens;

namespace Mavim.Libraries.Authorization.Models
{
    public class AuthSchema
    {
        public string Name { get; set; }
        public string Audience { get; set; }
        public string TenantId { get; set; }
        public bool UseQueryString { get; set; }
        public TokenValidationParameters ValidationParms { get; set; }
        public bool IsDefault { get; set; }
    }
}
