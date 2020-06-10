using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace DFC.Compui.Telemetry.TelemetryInitializers
{
    public class ApplicationTelemetryInitializer : ITelemetryInitializer
    {
        private readonly ILogger<ApplicationTelemetryInitializer> _logger;
        private readonly IConfiguration _configuration;
        private string? applicationInstanceId;

        public ApplicationTelemetryInitializer(ILogger<ApplicationTelemetryInitializer> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;

            Setup();
        }

        public void Setup()
        {
            var applicationName = !string.IsNullOrEmpty(_configuration["Configuration:ApplicationName"]) ? _configuration["Configuration:ApplicationName"] : throw new ArgumentException($"Configuration:ApplicationName Key not found in configuration");

            applicationInstanceId = Guid.NewGuid().ToString();

            //Log to Console for App Service / K8S Tracing
            Console.WriteLine($"Application Name: {applicationName}");
            Console.WriteLine($"Application Instance Id: {applicationInstanceId}");

            _logger.LogInformation($"Application Name: {applicationName}");
            _logger.LogInformation($"Application Instance Id: {applicationInstanceId}");
        }

        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            var applicationName = _configuration["Configuration:ApplicationName"] ?? throw new ArgumentException($"Configuration:ApplicationName Key not found in configuration");

            //RoleName is used to distinguish instances in the AI Application Map
            //Pods in K8S will have a null value, so set to the instance ID
            if (string.IsNullOrWhiteSpace(telemetry.Context.Cloud.RoleName))
            {
                telemetry.Context.Cloud.RoleName = $"{applicationName}_{applicationInstanceId}";
            }

            //Add to Custom Properties in AI to allow correlation
            if (!telemetry.Context.GlobalProperties.ContainsKey("ApplicationName"))
            {
                telemetry.Context.GlobalProperties.Add("ApplicationName", applicationName);
            }
            if (!telemetry.Context.GlobalProperties.ContainsKey("ApplicationInstanceId"))
            {
                telemetry.Context.GlobalProperties.Add("ApplicationInstanceId", applicationInstanceId!.ToString());
            }
        }
    }
}
