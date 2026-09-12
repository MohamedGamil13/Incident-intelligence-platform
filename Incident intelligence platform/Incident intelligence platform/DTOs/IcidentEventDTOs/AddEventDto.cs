using Incident_intelligence_platform.Enums;
using System.ComponentModel.DataAnnotations;

namespace Incident_intelligence_platform.DTOs.IcidentEventDTOs
{
    public class AddIncidentEventDto
    {
        public IncidentEventTimeStamp TimeStamp { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}