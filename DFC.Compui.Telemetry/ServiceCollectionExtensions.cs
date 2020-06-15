using DFC.Compui.Telemetry.HostedService;
using Microsoft.Extensions.DependencyInjection;

namespace DFC.Compui.Telemetry
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHostedServiceTelemetryWrapper(
          this IServiceCollection services)
        {
            services.AddTransient<IHostedServiceTelemetryWrapper, HostedServiceTelemetryWrapper>();
            return services;
        }
    }
}
