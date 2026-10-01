using Domain.Entities.Incidents;
using Domain.Enums.Incident;

namespace ServiceLayer.Services.Specifications
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
