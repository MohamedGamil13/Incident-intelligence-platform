using Domain.Entities.Incidents;
using Shared;

namespace ServiceLayer.Services.Specifications
{
    public class IncidentWithFilterForCountSpecification : BaseSpecification<Incident, int>
    {
        public IncidentWithFilterForCountSpecification(IncidentSpecParams specParams)
            : base(x =>
                (!specParams.ServiceId.HasValue || x.ServiceId == specParams.ServiceId.Value) &&
                (string.IsNullOrEmpty(specParams.Search) || x.Title.Contains(specParams.Search))
            )
        {
        }
    }
}
