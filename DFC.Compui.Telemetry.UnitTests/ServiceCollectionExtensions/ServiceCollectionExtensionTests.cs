﻿using DFC.Compui.Telemetry.HostedService;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Microsoft.Extensions.Configuration;

namespace DFC.Compui.Telemetry.UnitTests.ServiceCollectionExtensions
{
    public class ServiceCollectionExtensionTests
    {
        [Fact]
        public void ServiceCollectionExtensions_WhenAddHostedServiceTelemetryWrapperCalled_ServiceAdded()
        {
            //Arrange
            var serviceCollection = new ServiceCollection();

            //Act
            serviceCollection.AddHostedServiceTelemetryWrapper();

            //Assert
            Assert.Single(serviceCollection);
            Assert.Equal(typeof(IHostedServiceTelemetryWrapper), serviceCollection.FirstOrDefault().ServiceType);
        }

        [Fact]
        public void ServiceCollectionExtensions_WhenAddHostedServiceTelemetryWrapperCalled_ServiceResolved()
        {
            //Arrange
            var serviceCollection = new ServiceCollection();

            //Act
            var fakeConfiguration = A.Fake<IConfiguration>();
            serviceCollection.AddSingleton(fakeConfiguration);
            serviceCollection.AddHostedServiceTelemetryWrapper();
            var provider = serviceCollection.BuildServiceProvider();

            var telemetryWrapper = provider.GetService<IHostedServiceTelemetryWrapper>();

            //Assert
            Assert.NotNull(telemetryWrapper);
        }
    }
}
