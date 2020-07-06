using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DFC.Compui.Telemetry.HostedService
{
    public class HostedServiceTelemetryWrapper : IHostedServiceTelemetryWrapper
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<HostedServiceTelemetryWrapper> logger;

        public HostedServiceTelemetryWrapper(IConfiguration configuration, ILogger<HostedServiceTelemetryWrapper> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task Execute(Func<Task> serviceToExecute, string hostedServiceName)
        {
            if (serviceToExecute == null)
            {
                throw new ArgumentNullException(nameof(serviceToExecute));
            }

            TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();

            configuration.InstrumentationKey = this.configuration["APPINSIGHTS_INSTRUMENTATIONKEY"] ?? throw new ArgumentException($"APPINSIGHTS_INSTRUMENTATIONKEY");
            configuration.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());

            var telemetryClient = new TelemetryClient(configuration);

            var activity = new Activity(hostedServiceName).Start();

            try
            {
                var module = new DependencyTrackingTelemetryModule();
                module.Initialize(configuration);

                using (module)
                {
                    telemetryClient.StartOperation<DependencyTelemetry>($"Hosted Service Operation: {hostedServiceName} - {serviceToExecute.Method.Name}");
                    telemetryClient.TrackTrace($"Tracking {hostedServiceName} - {serviceToExecute.Method.Name}");

                    await Task.Run(() => serviceToExecute.Invoke()).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                throw;
            }
            finally
            {
                activity.Stop();
            }

            // before exit, flush the remaining data
            telemetryClient.Flush();

            // flush is not blocking when not using InMemoryChannel so wait a bit. There is an active issue regarding the need for `Sleep`/`Delay`
            // which is tracked here: https://github.com/microsoft/ApplicationInsights-dotnet/issues/407
            Task.Delay(10000).Wait();
        }
    }
}