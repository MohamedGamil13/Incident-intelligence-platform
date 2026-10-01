using Domain.Contracts.Auth;
using Domain.Contracts.Incidents;
using Domain.Contracts.Logs;
using Domain.Contracts.ServiceDeployments;
using Domain.Contracts.Services;
using Domain.Entities;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {

        IGenericRepo<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
        ILogsRepo LogsRepo { get; }
        IServiceDeploymentsRepo ServiceDeploymentsRepo { get; }
        IServiceRepo ServiceRepo { get; }
        IIncidentRepo IncidentRepo { get; }
        IAuthRepo AuthRepo { get; }
        IUserMangementRepo UserMangementRepo { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
