using Incident_intelligence_platform.Models;
using System.ComponentModel.DataAnnotations;

namespace Incident_intelligence_platform.DTOs.IcidentEventDTOs
{
    public class AddIncidentEventResponse
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public int IncidentId { get; set; }
        [Required]
        public string IncidentName { get; set; } = string.Empty;

        [Required]
        public IncidentSeverity IncidentSeverity { get; set; }
        [Required]
        public IncidentStatus IcidentStatus { get; set; }
        [Required]
        public string IncidentDate { get; set; } = string.Empty;

    }
}
