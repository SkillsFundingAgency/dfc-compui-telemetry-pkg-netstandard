using FakeItEasy;
using DFC.Compui.Telemetry.HostExtensions;
using Microsoft.ApplicationInsights.Extensibility;
using Xunit;
using System.Linq;
using DFC.Compui.Telemetry.TelemetryInitializers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;

namespace DFC.Compui.Telemetry.UnitTests.HostExtensions
{
    public class HostExtensionsTests
    {
        [Fact]
        public void HostExtensions_WhenAddApplicationTelemetryInitializer_AddsToTelemetryCollection()
        {
            //Arrange
            var configuration = A.Fake<IConfiguration>();
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("TestAppA");

            var webHost = A.Fake<IWebHost>();
            A.CallTo(() => webHost.Services.GetService(typeof(TelemetryConfiguration))).Returns(new TelemetryConfiguration());
            A.CallTo(() => webHost.Services.GetService(typeof(ILogger<ApplicationTelemetryInitializer>))).Returns(A.Fake<ILogger<ApplicationTelemetryInitializer>>());
            A.CallTo(() => webHost.Services.GetService(typeof(IConfiguration))).Returns(configuration);

            //Act
            webHost.AddApplicationTelemetryInitializer();

            //Assert
            var telemetryConfig = (TelemetryConfiguration)webHost.Services.GetService(typeof(TelemetryConfiguration));

            Assert.Single(telemetryConfig.TelemetryInitializers);
            Assert.True(telemetryConfig.TelemetryInitializers.Any(x => x.GetType() == typeof(ApplicationTelemetryInitializer)));
        }

        [Fact]
        public void HostExtensions_WhenAddApplicationTelemetryInitializer_ThrowsTelemetryNullException()
        {
            //Arrange
            var configuration = A.Fake<IConfiguration>();
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("TestAppA");

            var webHost = A.Fake<IWebHost>();
            A.CallTo(() => webHost.Services.GetService(typeof(TelemetryConfiguration))).Returns(null);
            A.CallTo(() => webHost.Services.GetService(typeof(ILogger<ApplicationTelemetryInitializer>))).Returns(A.Fake<ILogger<ApplicationTelemetryInitializer>>());
            A.CallTo(() => webHost.Services.GetService(typeof(IConfiguration))).Returns(configuration);

            //Act
            //Assert
            Assert.Throws<ArgumentException>(() => webHost.AddApplicationTelemetryInitializer());
        }

        [Fact]
        public void HostExtensions_WhenAddApplicationTelemetryInitializer_ThrowsLoggerNullException()
        {
            //Arrange
            var configuration = A.Fake<IConfiguration>();
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("TestAppA");

            var webHost = A.Fake<IWebHost>();
            A.CallTo(() => webHost.Services.GetService(typeof(TelemetryConfiguration))).Returns(new TelemetryConfiguration());
            A.CallTo(() => webHost.Services.GetService(typeof(ILogger<ApplicationTelemetryInitializer>))).Returns(null);
            A.CallTo(() => webHost.Services.GetService(typeof(IConfiguration))).Returns(configuration);

            //Act
            //Assert
            Assert.Throws<ArgumentException>(() => webHost.AddApplicationTelemetryInitializer());
        }


        [Fact]
        public void HostExtensions_WhenAddApplicationTelemetryInitializer_ThrowsConfigurationNullException()
        {
            //Arrange
            var webHost = A.Fake<IWebHost>();
            A.CallTo(() => webHost.Services.GetService(typeof(TelemetryConfiguration))).Returns(new TelemetryConfiguration());
            A.CallTo(() => webHost.Services.GetService(typeof(ILogger<ApplicationTelemetryInitializer>))).Returns(A.Fake<ILogger<ApplicationTelemetryInitializer>>());
            A.CallTo(() => webHost.Services.GetService(typeof(IConfiguration))).Returns(null);

            //Act
            //Assert
            Assert.Throws<ArgumentException>(() => webHost.AddApplicationTelemetryInitializer());
        }
    }
}
