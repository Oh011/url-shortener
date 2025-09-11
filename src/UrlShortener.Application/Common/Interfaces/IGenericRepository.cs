using Project.Domain.Specifications;
using System.Linq.Expressions;

namespace Project.Application.Common.Interfaces
{
    public interface IGenericRepository<T, TKey> where T : class
    {


        Task<T?> GetById(TKey id);

        Task AddAsync(T item);



        Task<IEnumerable<T>> GetAllWithSpecifications(
            BaseSpecifications<T> specification,
             bool asNoTracking = true);



        Task<List<TResult>> GetFilteredPaginatedAsync<TResult>(
   Expression<Func<T, bool>>? predicate = null,
   Expression<Func<T, object>>? sort = null,
   Expression<Func<T, TResult>>? selector = null,
   int? take = null,
   int? skip = null,
   bool ascending = true,
   CancellationToken cancellationToken = default);


        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);

        void UpdateRange(IEnumerable<T> items);

        void Delete(T item);



        Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            bool asNoTracking = true);




        Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            bool asNoTracking = false);

        Task<TResult?> FirstOrDefaultAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TResult>> selector,
            bool asNoTracking = true);

        Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true);


        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
