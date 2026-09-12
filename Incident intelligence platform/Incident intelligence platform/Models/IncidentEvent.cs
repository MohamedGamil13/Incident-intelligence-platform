using Incident_intelligence_platform.Enums;
using System.ComponentModel.DataAnnotations;

namespace Incident_intelligence_platform.Models
{
    public class IncidentEvent
    {
        public int Id { get; set; }

        public IncidentEventTimeStamp TimeStamp { get; set; }

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public int IncidentId { get; set; }
    }
}