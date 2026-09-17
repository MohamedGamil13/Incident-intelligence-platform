using Domain.Entities.Incidents;

namespace Domain.Entities.Services
{
    public class Service
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Incident>? Incidents { get; set; }
    }
}