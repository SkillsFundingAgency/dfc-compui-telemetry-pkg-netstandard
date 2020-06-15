using DFC.Compui.Telemetry.Models;
using DFC.Compui.Telemetry.TraceExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace DFC.Compui.Telemetry.UnitTests.TraceExtensions
{
    public class TraceExtensionsTests
    {
        [Fact]
        public void DoSomething()
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
