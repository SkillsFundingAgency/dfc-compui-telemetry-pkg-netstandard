using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Compui.Telemetry.HostedService
{
    public class HostedServiceTelemetryWrapper : IHostedServiceTelemetryWrapper
    {
        private readonly IConfiguration _configuration;

        public HostedServiceTelemetryWrapper(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task Execute(Func<Task> serviceToExecute, string hostedServiceName)
        {
            if (serviceToExecute == null)
            {
                throw new ArgumentNullException(nameof(serviceToExecute));
            }

            TelemetryConfiguration configuration = TelemetryConfiguration.CreateDefault();

            configuration.InstrumentationKey = _configuration["ApplicationInsights:InstrumentationKey"] ?? throw new ArgumentException($"ApplicationInsights:Instrumentation Key not found in configuration");
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
            catch (Exception)
            {
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