namespace DFC.Compui.Telemetry.Interface
{
    public interface IRequestTrace
    {
        public string? TraceId { get; }

        public string? ParentId { get; }

        void AddParentId(string parentId);

        void AddTraceId(string traceId);
    }
}
