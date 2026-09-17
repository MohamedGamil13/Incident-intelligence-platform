using Domain.Entities.Services;
using Domain.Enums.Incident;

namespace Domain.Entities.Incidents
{
    public class Incident
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int ServiceId { get; set; }
        public IncidentSeverity Severity { get; set; }
        public IncidentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }
        public Service? Service { get; set; }

    }


}
