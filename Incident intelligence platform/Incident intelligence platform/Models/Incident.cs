namespace Incident_intelligence_platform.Models
{
    public class Incident
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }
        public int ServiceId { get; set; }
        public IncidentSeverity Severity { get; set; }
        public IncidentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }


    }
    public enum IncidentSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum IncidentStatus
    {
        Open,
        Investigating,
        Mitigated,
        Resolved
    }
}
