// <copyright file="HostExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.Compui.Telemetry.TelemetryInitializers;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace DFC.Compui.Telemetry.HostExtensions
{
    public static class HostExtensions
    {
        public static IWebHost AddApplicationTelemetryInitializer(this IWebHost host)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            var telemetryConfig = (TelemetryConfiguration)host.Services.GetService(typeof(TelemetryConfiguration)) ?? throw new ArgumentException($"{nameof(TelemetryConfiguration)} not present in host services");
            var logger = (ILogger<ApplicationTelemetryInitializer>)host.Services.GetService(typeof(ILogger<ApplicationTelemetryInitializer>)) ?? throw new ArgumentException($"{nameof(ILogger<ApplicationTelemetryInitializer>)} not present in host services");
            var configuration = (IConfiguration)host.Services.GetService(typeof(IConfiguration)) ?? throw new ArgumentException($"{nameof(IConfiguration)} not present in host services");
            telemetryConfig.TelemetryInitializers.Add(new ApplicationTelemetryInitializer(logger, configuration));

            return host;
        }

        public static IHost AddApplicationTelemetryInitializer(this IHost host)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            var telemetryConfig = (TelemetryConfiguration)host.Services.GetService(typeof(TelemetryConfiguration)) ?? throw new ArgumentException($"{nameof(TelemetryConfiguration)} not present in host services");
            var logger = (ILogger<ApplicationTelemetryInitializer>)host.Services.GetService(typeof(ILogger<ApplicationTelemetryInitializer>)) ?? throw new ArgumentException($"{nameof(ILogger<ApplicationTelemetryInitializer>)} not present in host services");
            var configuration = (IConfiguration)host.Services.GetService(typeof(IConfiguration)) ?? throw new ArgumentException($"{nameof(IConfiguration)} not present in host services");
            telemetryConfig.TelemetryInitializers.Add(new ApplicationTelemetryInitializer(logger, configuration));

            return host;
        }
    }
}
