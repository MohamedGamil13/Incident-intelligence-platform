using Domain.Entities.Services;
using ServiceLayer.Services.Specifications;

namespace ServiceLayer.Specifications.ServiceSpecifications
{
    public class ServiceSpecification : BaseSpecification<Service, int>
    {
        public ServiceSpecification(int pageNumber, int pageSize)
        {
            AddOrderBy(s => s.Id);
            ApplyPagination(pageSize, pageNumber);
        }
    }
}