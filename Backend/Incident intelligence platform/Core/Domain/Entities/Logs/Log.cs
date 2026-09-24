using Domain.Entities.Services;
using Domain.Enums.Logs;

namespace Domain.Entities.Logs
{
    public class Log
    {
        public int Id { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public Guid TraceId { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public long LatencyMs { get; set; }

    }
}
