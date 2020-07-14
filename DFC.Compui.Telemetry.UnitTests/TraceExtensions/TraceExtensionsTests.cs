using DFC.Compui.Telemetry.Models;
using DFC.Compui.Telemetry.TraceExtensions;
using System;
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
            var activity = new Activity("TestOperation1").Start();
            model.AddTraceInformation();

            // Assert
            Assert.NotNull(model.TraceId);
            Assert.NotNull(model.ParentId);
        }

        [Fact]
        public void RequestTrace_OnAddTraceInformation_ThrowsArgumentException()
        {
            // Arrange
            var model = (RequestTrace)null;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => model.AddTraceInformation());
        }

        [Fact]
        public void RequestTrace_OnAddTraceInformation_ThrowsInvalidOperationException()
        {
            // Arrange
            var model = new RequestTrace();

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => model.AddTraceInformation());
        }
    }
}
