using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Compui.Telemetry.HostedService
{
    public interface IHostedServiceTelemetryWrapper
    {
        Task Execute(Func<Task> serviceToExecute, string hostedServiceName);
    }
}
