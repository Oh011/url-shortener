using Microsoft.EntityFrameworkCore;
using Project.Application.Common.Interfaces;
using Project.Domain.Specifications;
using System.Linq.Expressions;

namespace Project.Infrastructure.Persistence.Repositories
{
    internal class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : class
    {


        private readonly DbContext _context;


        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T item)
        {

            await _context.Set<T>().AddAsync(item);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {

            await _context.Set<T>().AddRangeAsync(entities);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {

            return await _context.Set<T>().CountAsync(predicate);
        }

        public void Delete(T item)
        {
            _context.Set<T>().Remove(item);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().AnyAsync(predicate);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true)
        {
            var query = _context.Set<T>().Where(predicate);
            return asNoTracking ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();
        }

        public async Task<TResult?> FirstOrDefaultAsync<TResult>(
          Expression<Func<T, bool>> predicate,
          Expression<Func<T, TResult>> selector,
          bool asNoTracking = true)
        {
            var query = _context.Set<T>().Where(predicate);

            if (asNoTracking) query = query.AsNoTracking();
            return await query.Select(selector).FirstOrDefaultAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false)
        {

            var query = _context.Set<T>().Where(predicate);

            if (asNoTracking) query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = true)
            => asNoTracking
                ? await _context.Set<T>().AsNoTracking().ToListAsync()
                : await _context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllWithSpecifications(
            BaseSpecifications<T> specification,
            bool asNoTracking = true)
        {


            var query = ApplyBaseSpecifications(specification);
            return asNoTracking ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();

        }





        private IQueryable<T> ApplyBaseSpecifications(BaseSpecifications<T> specifications)
        {

            return BaseSpecificationsEvaluator<T>.GetQuery(_context.Set<T>(), specifications);
        }

        public async Task<T?> GetById(TKey id)
         => await _context.Set<T>().FindAsync(id);



        public async Task<List<TResult>> GetFilteredPaginatedAsync<TResult>(
      Expression<Func<T, bool>>? predicate = null,
      Expression<Func<T, object>>? sort = null,
      Expression<Func<T, TResult>>? selector = null,
      int? take = null,
      int? skip = null,
      bool ascending = true,
      CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();

            // Filtering
            if (predicate != null)
                query = query.Where(predicate);

            // Sorting
            if (sort != null)
                query = ascending ? query.OrderBy(sort) : query.OrderByDescending(sort);

            // Paging
            if (skip.HasValue)
                query = query.Skip(skip.Value);
            if (take.HasValue)
                query = query.Take(take.Value);

            // Projection
            if (selector != null)
                return await query.Select(selector).ToListAsync(cancellationToken);
            else
                // If no selector, cast to TResult assuming T == TResult
                return await query.Cast<TResult>().ToListAsync(cancellationToken);
        }


        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void UpdateRange(IEnumerable<T> items) => _context.Set<T>().UpdateRange(items);

    }
}
