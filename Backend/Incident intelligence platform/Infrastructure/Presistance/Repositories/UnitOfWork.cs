using Domain.Contracts;
using Domain.Contracts.Auth;
using Domain.Contracts.Incidents;
using Domain.Contracts.Logs;
using Domain.Contracts.ServiceDeployments;
using Domain.Contracts.Services;
using Domain.Entities;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Persistence.Repositories.Auth;
using Presistance.Repositories.Auth;
using Presistance.Repositories.Logs;
using Presistance.Repositories.ServiceDeployments;
using Presistance.Repositories.Services;
using System.Collections.Concurrent;

namespace Presistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbcontext _context;
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly ConcurrentDictionary<string, object> _repositories;
        private readonly RoleManager<IdentityRole> _roleManager;

        private ILogsRepo? _logsRepo;
        private IServiceDeploymentsRepo? _deploymentsRepo;
        private IServiceRepo? _serviceRepo;
        private IIncidentRepo? _incidentRepo;
        private IAuthRepo? _authRepo;
        private IUserMangementRepo? _userMangementRepo;

        public UnitOfWork(AppDbcontext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<string, object>();
            _userManger = userManager;
            _roleManager = roleManager;
        }

        public IGenericRepo<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            return (IGenericRepo<TEntity, TKey>)_repositories.GetOrAdd(
                typeof(TEntity).Name,
                _ => new GenericRepo<TEntity, TKey>(_context)
            );
        }


        public ILogsRepo LogsRepo => _logsRepo ??= new LogsRepo(_context);
        public IServiceDeploymentsRepo ServiceDeploymentsRepo => _deploymentsRepo ??= new ServiceDeploymentsRepo(_context);
        public IServiceRepo ServiceRepo => _serviceRepo ??= new ServiceRepository(_context);
        public IIncidentRepo IncidentRepo => _incidentRepo ??= new IncidentRepo(_context);
        public IAuthRepo AuthRepo => _authRepo ??= new AuthRepo(_userManger);
        public IUserMangementRepo UserMangementRepo => _userMangementRepo ??= new UserMangementRepo(_context, _userManger, _roleManager);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
