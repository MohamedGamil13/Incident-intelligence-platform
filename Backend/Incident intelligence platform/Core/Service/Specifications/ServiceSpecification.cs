using Domain.Entities.Services;
using ServiceLayer.Services.Specifications;

namespace ServiceLayer.Specifications
{
    public class ServiceSpecification : BaseSpecification<Service, int>
    {

        public ServiceSpecification(int pageNumber, int pageSize)
        {
            ApplyPagination((pageNumber - 1) * pageSize, pageSize);
        }


        public ServiceSpecification(int id) : base(s => s.Id == id)
        {
        }
    }
}
