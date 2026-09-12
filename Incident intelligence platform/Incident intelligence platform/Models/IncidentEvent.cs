using Incident_intelligence_platform.Enums;

namespace Incident_intelligence_platform.Models
{
    public class IncidentEvent
    {
        public int Id { get; set; }

        public IncidentEventTimeStamp TimeStamp { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public int IncidentId { get; set; }
    }

}
