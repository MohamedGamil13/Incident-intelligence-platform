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
}