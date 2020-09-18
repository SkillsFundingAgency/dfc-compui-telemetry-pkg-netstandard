namespace DFC.Compui.Telemetry.Interface
{
    public interface IRequestTrace
    {
        public string? TraceId { get; }

        public string? ParentId { get; }
    }
}
