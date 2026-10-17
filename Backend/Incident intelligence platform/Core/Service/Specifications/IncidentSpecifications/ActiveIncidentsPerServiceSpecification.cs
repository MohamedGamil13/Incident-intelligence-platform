using Domain.Entities.Incidents;
using Domain.Enums.Incident;
using ServiceLayer.Services.Specifications;

namespace ServiceLayer.Specifications.IncidentSpecifications
{
    public class ActiveIncidentsPerServiceSpecification : BaseSpecification<Incident, int>
    {
        public ActiveIncidentsPerServiceSpecification(int serviceId, int timeWindowInMin = 5)
            : base(i => i.ServiceId == serviceId &&
                        i.CreatedAt >= DateTime.UtcNow.AddMinutes(-timeWindowInMin) &&
                        i.Status == IncidentStatus.Open)
        {
            AddOrderByDescending(i => i.CreatedAt);
        }
    }
}
