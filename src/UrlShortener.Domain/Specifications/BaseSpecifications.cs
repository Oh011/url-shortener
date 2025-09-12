using System.Linq.Expressions;

namespace Project.Domain.Specifications
{
    public abstract class BaseSpecifications<T>
    {

        public Expression<Func<T, bool>>? Criteria { get; private set; }

        public List<Expression<Func<T, object>>> IncludeExpressions { get; } = new();


        public Expression<Func<T, object>>? OrderBy { get; set; }

        public Expression<Func<T, object>>? OrderByDescending { get; set; }

        public int Take { get; set; }

        public int Skip { get; set; }

        public bool isPaginated { get; set; } = false;

        public bool IgnoreQueryFilters { get; set; } = true;



        // Constructor
        protected BaseSpecifications(Expression<Func<T, bool>>? criteria = null)
        {
            Criteria = criteria;
        }

        // Include
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
            => IncludeExpressions.Add(includeExpression);

        //// ThenInclude - optional extension






        // Ordering
        protected void SetOrderBy(Expression<Func<T, object>> expression)
            => OrderBy = expression;

        protected void SetOrderByDescending(Expression<Func<T, object>> expression)
            => OrderByDescending = expression;

        // Pagination
        protected void ApplyPagination(int pageIndex, int pageSize)
        {
            isPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }
    }
}
