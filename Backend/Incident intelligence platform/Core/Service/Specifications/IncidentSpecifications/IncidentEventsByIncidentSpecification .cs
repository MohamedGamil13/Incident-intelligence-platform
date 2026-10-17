using Domain.Entities.Incidents;
using ServiceLayer.Services.Specifications;

namespace ServiceLayer.Specifications.IncidentSpecifications
{
    public class IncidentEventsByIncidentSpecification : BaseSpecification<IncidentEvent, int>
    {
        public IncidentEventsByIncidentSpecification(int incidentId, int pageSize, int pageNumber)
            : base(e => e.IncidentId == incidentId)
        {
            AddOrderBy(e => e.Date);
            ApplyPagination(pageSize, pageNumber);
        }
    }
}