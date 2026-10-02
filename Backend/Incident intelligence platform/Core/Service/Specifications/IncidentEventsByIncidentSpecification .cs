using Domain.Entities.Incidents;

namespace ServiceLayer.Services.Specifications
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