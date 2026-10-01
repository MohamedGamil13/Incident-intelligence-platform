using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Contracts
{
    public interface ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>>? WhereExpression { get; }

        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; }
        public Expression<Func<TEntity, object>>? OrderByExpressions { get; }
        public Expression<Func<TEntity, object>>? OrderByDescExpressions { get; }

        public int Skip { get; }
        public int Take { get; }
        public bool IsPaginated { get; }
    }
}
