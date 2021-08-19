using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.Int.ChLog.Relationship.Extensions
{
    public static class ApplicationInsightsExtensions
    {
        /// <summary>
        /// Adds the application insights.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="isDevelopment"></param>
        public static void AddApplicationInsights(this IServiceCollection services, bool isDevelopment)
        {
            if (!isDevelopment)
            {
                //TODO: We need to use telemetry key from the application insights to pass it as an argument, but need to see how to do it the best way instead of using appsettings.json (WI: 17420)
                services.AddApplicationInsightsTelemetry();
                services.AddApplicationInsightsKubernetesEnricher();
            }
        }
    }
}
