using Domain.Entities.Incidents;
using Shared;

namespace ServiceLayer.Services.Specifications
{
    public class IncidentWithDetailsSpecification : BaseSpecification<Incident, int>
    {

        public IncidentWithDetailsSpecification(IncidentSpecParams specParams)
            : base(x =>
                (!specParams.ServiceId.HasValue || x.ServiceId == specParams.ServiceId.Value) &&
                (string.IsNullOrEmpty(specParams.Search) || x.Title.Contains(specParams.Search))
            )
        {
            AddInclude(x => x.Service!);


            AddOrderByDescending(x => x.CreatedAt);


            ApplyPagination(specParams.PageSize, specParams.PageIndex);
        }


        public IncidentWithDetailsSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Service!);
        }
    }

}
