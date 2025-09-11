using Microsoft.EntityFrameworkCore;
using Project.Application.Common.Interfaces;
using Project.Infrastructure.Persistence.Context;
using Shared.Options;
using System.Collections.Concurrent;

namespace Project.Infrastructure.Persistence.Repositories
{
    internal class ShardUnitOfWork : IUnitOfWork
    {


        private readonly UrlDbContext _context;
        private readonly ConcurrentDictionary<string, object> _Repositories;


        public ShardUnitOfWork(ShardInfo info)
        {



            var options = new DbContextOptionsBuilder<UrlDbContext>()
                .UseNpgsql(info.ConnectionString)
                .Options;

            _context = new UrlDbContext(options);

            _Repositories = new ConcurrentDictionary<string, object>();


        }

        public async Task<int> Commit(CancellationToken cancellationToken = default)
        {

            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {

            _context.Dispose();
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class
        {
            return (IGenericRepository<TEntity, TKey>)_Repositories.GetOrAdd(typeof(TEntity).FullName, _ => new GenericRepository<TEntity, TKey>(_context));
        }


    }
}
