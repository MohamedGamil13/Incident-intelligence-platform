using Domain.Enums.Incident;

namespace Domain.Entities.Incidents
{
    public class IncidentEvent : BaseEntity<int>
    {
        public IncidentEventTimeStamp TimeStamp { get; set; }
        public string Description { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public int IncidentId { get; set; }
        public Incident? Incident { get; set; }
    }
}