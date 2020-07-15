using DFC.Compui.Telemetry.ApplicationBuilderExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DFC.Compui.Telemetry.UnitTests.ApplicationBuilderExtensions
{
    public class ApplicationBuilderExtensionsTests
    {
        [Fact]
        public void DoSometing()
        {
            // Arrange
            var applicationBuilder = new ApplicationBuilder(new ServiceCollection().BuildServiceProvider());

            // Act
            applicationBuilder.AddOperationIdToRequests();

            // Assert - does nothing
        }
    }
}
