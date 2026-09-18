using Domain.Enums.Logs;

namespace Shared.Dtos.Logs
{
    public class LogResponseDto
    {
        public int Id { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public Guid TraceId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
    }
}
