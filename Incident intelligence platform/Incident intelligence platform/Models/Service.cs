using Incident_intelligence_platform.Models;

public class Service
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Incident>? Incidents { get; set; }
}