using Incident_intelligence_platform.Models;
using System.ComponentModel.DataAnnotations;

namespace Incident_intelligence_platform.DTOs
{
    public class UpdateIncidentRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

        public required IncidentSeverity Severity { get; set; }
        public required IncidentStatus Status { get; set; }
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
        public required string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Severity level is required.")]
        public IncidentSeverity Severity { get; set; }

        [Required(ErrorMessage = "ServiceId is required.")]
        public int ServiceId { get; set; }
    }
}