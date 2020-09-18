using DFC.Compui.Telemetry.Interface;
using System;

namespace DFC.Compui.Telemetry.Models
{
    public class RequestTrace : IRequestTrace
    {
        public string? TraceId { get; private set; }

        public string? ParentId { get; private set; }

        public void AddTraceId(string traceId)
        {
            if (string.IsNullOrWhiteSpace(traceId))
            {
                throw new ArgumentException($"{nameof(this.TraceId)} cannot be null");
            }

            this.TraceId = traceId;
        }

        public void AddParentId(string parentId)
        {
            if (string.IsNullOrWhiteSpace(parentId))
            {
                throw new ArgumentException($"{nameof(this.TraceId)} cannot be null");
            }

            this.ParentId = parentId;
        }
    }
}
