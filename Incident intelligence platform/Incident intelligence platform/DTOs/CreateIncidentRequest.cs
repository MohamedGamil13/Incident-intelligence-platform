using Incident_intelligence_platform.Models;
using System.ComponentModel.DataAnnotations;

namespace Incident_intelligence_platform.DTOs
{
    public class CreateIncidentRequestDTO
    {

        [MaxLength(100)]
        public required string Title { get; set; }
        [MaxLength(200)]
        public required string Description { get; set; }
        public required int ServiceId { get; set; }
        public required IncidentSeverity Severity { get; set; }

    }
}
