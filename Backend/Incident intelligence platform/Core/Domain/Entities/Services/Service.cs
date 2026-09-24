using Domain.Entities.Incidents;
using Domain.Entities.Logs;
using Domain.Entities.ServiceDeployments;

namespace Domain.Entities.Services
{
    public class Service
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Incident>? Incidents { get; set; }
        public ICollection<Log>? Logs { get; set; }
        public ICollection<ServiceDeployment>? ServiceDepolyments { get; set; }

    }
}