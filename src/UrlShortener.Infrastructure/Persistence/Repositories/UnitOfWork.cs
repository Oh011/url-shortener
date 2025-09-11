using Project.Application.Common.Interfaces;
using Project.Infrastructure.Persistence.Context;
using System.Collections.Concurrent;

namespace Project.Infrastructure.Persistence.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {


        private ConcurrentDictionary<string, object> Repositories;
        private readonly ApplicationDbContext _context;



        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Repositories = new ConcurrentDictionary<string, object>();
        }



        public async Task<int> Commit(CancellationToken cancellationToken = default)
        {



            return await _context.SaveChangesAsync(cancellationToken);


        }




        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class
        {
            return (IGenericRepository<TEntity, TKey>)Repositories.GetOrAdd(typeof(TEntity).Name, (string key) => new GenericRepository<TEntity, TKey>(_context));
        }


        public void Dispose()
        {

            _context.Dispose();
        }


    }
}
