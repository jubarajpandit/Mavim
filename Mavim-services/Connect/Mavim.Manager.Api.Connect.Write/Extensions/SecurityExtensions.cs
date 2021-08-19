using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Mavim.Manager.Api.Connect.Write.Extensions
{
    /// <summary>
    /// Extension Security
    /// </summary>
    public static class SecurityExtensions
    {
        /// <summary>
        /// Adds the security.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public static void AddSecurity(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
                app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        }
    }
}
