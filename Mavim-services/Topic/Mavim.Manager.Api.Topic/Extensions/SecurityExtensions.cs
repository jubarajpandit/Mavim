using Microsoft.AspNetCore.Builder;

namespace Mavim.Manager.Api.Topic.Extensions
{
    /// <summary>
    /// SecurityExtensions
    /// </summary>
    public static class SecurityExtensions
    {
        /// <summary>
        /// Adds the security
        /// </summary>
        /// <param name="app"></param>
        /// <param name="isDevelopment"></param>
        public static void AddSecurity(this IApplicationBuilder app, bool isDevelopment)
        {
            if (!isDevelopment)
                app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

            app.ConfigureCors();
        }
    }
}
