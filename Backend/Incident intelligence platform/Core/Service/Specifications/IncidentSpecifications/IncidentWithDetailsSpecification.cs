using Domain.Entities.Incidents;
using ServiceLayer.Services.Specifications;
using Shared;
using Shared.Enums;

namespace ServiceLayer.Specifications.IncidentSpecifications
{
    public class IncidentWithDetailsSpecification : BaseSpecification<Incident, int>
    {
        public IncidentWithDetailsSpecification(IncidentSpecParams specParams)
            : base(x =>
                (!specParams.ServiceId.HasValue || x.ServiceId == specParams.ServiceId.Value) &&
                (!specParams.Status.HasValue || x.Status == specParams.Status.Value) &&
                (string.IsNullOrEmpty(specParams.Search) || x.Title.ToLower().Contains(specParams.Search.ToLower()))
            )
        {
            AddInclude(x => x.Service!);

            if (specParams.Sort.HasValue)
            {
                switch (specParams.Sort.Value)
                {
                    case IncidentSort.CreatedAtAsc:
                        AddOrderBy(x => x.CreatedAt);
                        break;

                    case IncidentSort.SeverityDesc:
                        AddOrderByDescending(x => x.Severity);
                        break;

                    case IncidentSort.SeverityAsc:
                        AddOrderBy(x => x.Severity);
                        break;

                    case IncidentSort.TitleAsc:
                        AddOrderBy(x => x.Title);
                        break;

                    case IncidentSort.TitleDesc:
                        AddOrderByDescending(x => x.Title);
                        break;

                    case IncidentSort.StatusAsc:
                        AddOrderBy(x => x.Status);
                        break;

                    case IncidentSort.StatusDesc:
                        AddOrderByDescending(x => x.Status);
                        break;

                    case IncidentSort.CreatedAtDesc:
                    default:
                        AddOrderByDescending(x => x.CreatedAt);
                        break;
                }
            }
            else
            {
                AddOrderByDescending(x => x.CreatedAt);
            }

            ApplyPagination(specParams.PageSize, specParams.PageIndex);
        }

        public IncidentWithDetailsSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Service!);
        }
    }
}