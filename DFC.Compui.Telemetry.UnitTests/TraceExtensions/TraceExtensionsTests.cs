using DFC.Compui.Telemetry.Models;
using DFC.Compui.Telemetry.TraceExtensions;
using System.Diagnostics;
using Xunit;

namespace DFC.Compui.Telemetry.UnitTests.TraceExtensions
{
    public class TraceExtensionsTests
    {
        [Fact]
        public void RequestTrace_OnAddTraceInformation_AddsTraceInformation()
        {
            // Arrange
            var model = new RequestTrace();

            // Act
            var activity = new Activity("TestOperation").Start();
            model.AddTraceInformation();

            // Assert
            Assert.NotNull(model.TraceId);
            Assert.NotNull(model.ParentId);
        }
    }
}
