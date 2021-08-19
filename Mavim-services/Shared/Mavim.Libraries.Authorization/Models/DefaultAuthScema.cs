using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Mavim.Libraries.Authorization.Models
{
    public class DefaultAuthSchema : AuthSchema
    {
        public DefaultAuthSchema()
        {
            Name = JwtBearerDefaults.AuthenticationScheme;
            IsDefault = true;
        }
    }
}
