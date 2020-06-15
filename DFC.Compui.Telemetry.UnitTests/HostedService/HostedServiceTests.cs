using DFC.Compui.Telemetry.HostedService;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Compui.Telemetry.UnitTests.HostedService
{
    public class HostedServiceTests
    {
        [Fact]
        public async Task HostedServiceTelemetryWrapper_WhenExecuteCalled_ExecutesEncapsulatedMethod()
        {
            // Arrange
            var configuration = A.Fake<IConfiguration>();
            A.CallTo(() => configuration["Configuration:ApplicationName"]).Returns("TestAppA");
            A.CallTo(() => configuration["ApplicationInsights:InstrumentationKey"]).Returns(Guid.NewGuid().ToString());

            var hostedService = new HostedServiceTelemetryWrapper(configuration);

            // Act
            var fakeSomeWork = A.Fake<Func<Task>>();
            await hostedService.Execute(() => fakeSomeWork(), "MyTestHostedService");

            // Assert
            A.CallTo(() => fakeSomeWork()).MustHaveHappenedOnceExactly();
        }
    }
}
