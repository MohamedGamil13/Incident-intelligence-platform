using Domain.Enums.Logs;

namespace Shared.Dtos.Logs
{
    public class CreateLogDto
    {
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ServiceId { get; set; }
    }
}
