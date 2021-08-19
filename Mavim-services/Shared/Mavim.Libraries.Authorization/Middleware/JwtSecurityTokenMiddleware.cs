using Mavim.Libraries.Authorization.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Consts = Mavim.Manager.Api.Utils.Constants;

namespace Mavim.Libraries.Authorization.Middleware
{
    public class JwtSecurityTokenMiddleware
    {
        private const string JwtUserIdKey = "oid"; // the object id in the jwt token is unique per AD user.
        private const string JwtNameKey = "name"; // the name in the jwt token is unique per AD user.
        private const string JwtEmailKey = "email";
        private const string JwtAudience = "tid";
        private const string JwtAppId = "aud";

        private readonly RequestDelegate _next;

        public JwtSecurityTokenMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context, IJwtSecurityToken token)
        {

            string rawToken = await context.GetTokenAsync(Consts.Configuration.TOKEN_NAME);

            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            if (jwtHandler.CanReadToken(rawToken))
            {
                JwtSecurityToken jwtSecurityToken = jwtHandler.ReadJwtToken(rawToken);

                token.RawData = rawToken;

                token.Token = jwtSecurityToken;

                token.UserID = jwtSecurityToken.Payload.ContainsKey(JwtUserIdKey)
                                ? Map(jwtSecurityToken.Payload[JwtUserIdKey].ToString())
                                : throw new Exception("Oid not found in the token.");

                token.TenantId = jwtSecurityToken.Payload.ContainsKey(JwtAudience)
                                ? Map(jwtSecurityToken.Payload[JwtAudience].ToString())
                                : throw new Exception("Tenant key not found in the token");

                token.AppId = jwtSecurityToken.Payload.ContainsKey(JwtAppId)
                                ? Map(jwtSecurityToken.Payload[JwtAppId].ToString())
                                : throw new Exception("ApplicationId key not found in the token");

                token.Name = jwtSecurityToken.Payload.ContainsKey(JwtNameKey)
                                ? jwtSecurityToken.Payload[JwtNameKey].ToString()
                                : throw new Exception("Name key not found in the token.");

                token.Email = jwtSecurityToken.Payload.ContainsKey(JwtEmailKey) &&
                                  new EmailAddressAttribute().IsValid(jwtSecurityToken.Payload[JwtEmailKey].ToString().ToLower())
                                ? jwtSecurityToken.Payload[JwtEmailKey].ToString().ToLower()
                                : throw new Exception("Email key not found in the token.");

            }

            await _next.Invoke(context);
        }

        private static Guid Map(string value) => Guid.TryParse(value, out Guid result)
                    ? result
                    : throw new Exception(string.Format("Payload {0} can not be converted to Guid", value));
    }
}
