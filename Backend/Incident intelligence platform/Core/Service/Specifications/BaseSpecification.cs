using Domain.Contracts;
using Domain.Entities;
using System.Linq.Expressions;

namespace ServiceLayer.Services.Specifications
{
    public abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected BaseSpecification() { }

        protected BaseSpecification(Expression<Func<TEntity, bool>>? whereExpression)
        {
            WhereExpression = whereExpression;
        }

        public Expression<Func<TEntity, bool>>? WhereExpression { get; private set; }
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; private set; } = new();
        public Expression<Func<TEntity, object>>? OrderByExpressions { get; private set; }
        public Expression<Func<TEntity, object>>? OrderByDescExpressions { get; private set; }

        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool IsPaginated { get; private set; } = false;

        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            IncludeExpressions.Add(includeExpression);
        }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderByExpressions = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            OrderByDescExpressions = orderByDescendingExpression;
        }

        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;
            IsPaginated = true;
        }
    }
}
