using Domain.Enums.Incident;
using System.ComponentModel.DataAnnotations;

namespace Incident_intelligence_platform.DTOs.IncidentDTOs
{
    public class UpdateIncidentRequestDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Severity level is required.")]
        [EnumDataType(typeof(IncidentSeverity), ErrorMessage = "Invalid severity level.")]
        public IncidentSeverity? Severity { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(IncidentStatus), ErrorMessage = "Invalid incident status.")]
        public IncidentStatus? Status { get; set; }
    }


    public class GetIncidentResponseDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public IncidentSeverity Severity { get; set; }
        public IncidentStatus Status { get; set; }
        public int ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }


    public class CreateIncidentRequestDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Severity level is required.")]
        [EnumDataType(typeof(IncidentSeverity), ErrorMessage = "Invalid severity level.")]
        public IncidentSeverity? Severity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ServiceId must be greater than 0.")]
        public int ServiceId { get; set; }
    }
}