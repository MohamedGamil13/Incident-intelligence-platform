using Domain.Contracts.Auth;
using Domain.Contracts.Logs;
using Domain.Entities;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {

        IGenericRepo<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
        ILogsRepo LogsRepo { get; }
        IAuthRepo AuthRepo { get; }
        IUserMangementRepo UserMangementRepo { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
