using System.ComponentModel.DataAnnotations;

namespace Incident_intelligence_platform.DTOs.IcidentEventDTOs
{
    public class AddIncidentEventDto
    {
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public int IncidentId { get; set; }

    }
}




