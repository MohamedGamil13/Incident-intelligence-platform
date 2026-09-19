using Domain.Contracts.ServiceDeployments;
using Domain.Entities.ServiceDeployments;
using Microsoft.EntityFrameworkCore;

namespace Presistance.Repositories.ServiceDeployments
{
    public class ServiceDeploymentsRepo : IServiceDeploymentsRepo
    {
        private readonly AppDbcontext context;

        public ServiceDeploymentsRepo(AppDbcontext context)
        {
            this.context = context;
        }
        public async Task AddAsync(ServiceDeployment deployment)
        {
            await context.AddAsync(deployment);
        }

        public async Task<IEnumerable<ServiceDeployment>> GetAllDeploymentByService(int serviceId)
        {
            return await context.ServiceDeployments
                .AsNoTracking()
                .Where(d => d.ServiceId == serviceId)
                .ToListAsync();

        }

        public async Task<ServiceDeployment?> GetLatestDeploymentByServiceIdAsync(int serviceId)
        {
            return await context.ServiceDeployments
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.ServiceId == serviceId);
        }
    }
}
