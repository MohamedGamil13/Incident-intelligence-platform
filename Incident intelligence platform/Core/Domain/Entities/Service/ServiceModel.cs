using Domain.Entities.Incident;
public class ServiceModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<IncidentModel>? Incidents { get; set; }
}