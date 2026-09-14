using Incident_intelligence_platform.Models;

namespace Incident_intelligence_platform.DTOs.IcidentEventDTOs
{
    public class AddIncidentEventResponse
    {
        public int IncidentId { get; set; }

        public string IncidentName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public IncidentStatus IcidentStatus { get; set; }

        public string IncidentDate { get; set; } = string.Empty;
    }
}