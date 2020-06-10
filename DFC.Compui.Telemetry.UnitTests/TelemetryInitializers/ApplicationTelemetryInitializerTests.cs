using DFC.Compui.Telemetry.TelemetryInitializers;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace DFC.Compui.Telemetry.UnitTests.TelemetryInitializers
{
    public class ApplicationTelemetryInitializerTests
    {
        [Fact]
        public void ApplicationTelemetryInitializer_WhenSetupCalled_ThrowsApplicationNameException()
        {
            //Arrange
            //Act
            //Assert
            Assert.Throws<ArgumentException>(() => new ApplicationTelemetryInitializer(A.Fake<ILogger<ApplicationTelemetryInitializer>>(), A.Fake<IConfiguration>()));
        }

        [Fact]
        public void ApplicationTelemetryInitializer_WhenSetupCalled_InitializesCorrectly()
        {
            //Arrange
            var configuration = A.Fake<IConfiguration>();
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("TestAppA");

            //Act
            var initializer = new ApplicationTelemetryInitializer(A.Fake<ILogger<ApplicationTelemetryInitializer>>(), configuration);

            //Assert
            A.CallTo(() => configuration["Configuration:ApplicationName"]).MustHaveHappened();
        }
    }
}
