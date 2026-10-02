using Domain.Entities.Services;

namespace ServiceLayer.Services.Specifications
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