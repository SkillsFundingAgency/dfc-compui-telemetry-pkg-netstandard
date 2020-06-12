using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace DFC.Compui.Telemetry.TelemetryInitializers
{
    public class ApplicationTelemetryInitializer : ITelemetryInitializer
    {
        private readonly ILogger<ApplicationTelemetryInitializer> logger;
        private readonly IConfiguration configuration;
        private string? applicationInstanceId;

        public ApplicationTelemetryInitializer(ILogger<ApplicationTelemetryInitializer> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;

            this.Setup();
        }

        public void Setup()
        {
            var applicationName = !string.IsNullOrEmpty(this.configuration["Configuration:ApplicationName"]) ? this.configuration["Configuration:ApplicationName"] : throw new ArgumentException($"Configuration:ApplicationName Key not found in configuration");

            this.applicationInstanceId = Guid.NewGuid().ToString();

            // Log to Console for App Service / K8S Tracing
            Console.WriteLine($"Application Name: {applicationName}");
            Console.WriteLine($"Application Instance Id: {this.applicationInstanceId}");

            this.logger.LogInformation($"Application Name: {applicationName}");
            this.logger.LogInformation($"Application Instance Id: {this.applicationInstanceId}");
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            var applicationName = this.configuration["Configuration:ApplicationName"] ?? throw new ArgumentException($"Configuration:ApplicationName Key not found in configuration");

            // RoleName is used to distinguish instances in the AI Application Map
            // Pods in K8S will have a null value, so set to the instance ID
            if (string.IsNullOrWhiteSpace(telemetry.Context.Cloud.RoleName))
            {
                telemetry.Context.Cloud.RoleName = $"{applicationName}_{this.applicationInstanceId}";
            }

            // Add to Custom Properties in AI to allow correlation
            if (!telemetry.Context.GlobalProperties.ContainsKey("ApplicationName"))
            {
                telemetry.Context.GlobalProperties.Add("ApplicationName", applicationName);
            }

            if (!telemetry.Context.GlobalProperties.ContainsKey("ApplicationInstanceId"))
            {
                telemetry.Context.GlobalProperties.Add("ApplicationInstanceId", this.applicationInstanceId!.ToString());
            }
        }
    }
}
