using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Presistance.Repositories
{
    public class GenericRepo<TEntity, TKey> : IGenericRepo<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly AppDbcontext context;

        public GenericRepo(AppDbcontext _context)
        {
            context = _context;
        }
        public async Task AddAsync(TEntity entity) => await context.Set<TEntity>().AddAsync(entity);
        public void Delete(TEntity entity) => context.Set<TEntity>().Remove(entity);
        public void Update(TEntity entity) => context.Set<TEntity>().Update(entity);
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false) => asNoTracking ? await context.Set<TEntity>().AsNoTracking().ToListAsync() : await context.Set<TEntity>().ToListAsync();
        public async Task<TEntity?> GetByIdAsync(TKey id) => await context.Set<TEntity>().FindAsync(id);
    }
}
