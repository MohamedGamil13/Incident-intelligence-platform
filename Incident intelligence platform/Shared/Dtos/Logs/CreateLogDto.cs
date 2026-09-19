using Domain.Enums.Logs;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Logs
{
    public class CreateLogDto
    {
        [Required(ErrorMessage = "Log level is required.")]
        [EnumDataType(typeof(LogLevel), ErrorMessage = "Invalid log level.")]
        public LogLevel? LogLevel { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [MaxLength(200, ErrorMessage = "Message cannot exceed 200 characters.")]
        public string Message { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "ServiceId must be greater than 0.")]
        public int ServiceId { get; set; }
    }
}