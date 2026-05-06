using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Persistence.Context;
using Shared.Options;
using System.Collections.Concurrent;
using UrlShortener.Application.Abstrcations.Persistence;

namespace UrlShortener.Infrastructure.Persistence.Repositories
{

    internal class ShardUnitOfWork : IShardUnitOfWork
    {
        private readonly UrlDbContext _context;

        public IGenericRepository<Url, long> UrlRepo { get; }
        public IGenericRepository<UrlAccessLog, string> UrlAccessLogRepo { get; }

        public ShardUnitOfWork(ShardInfo info)
        {
            var options = new DbContextOptionsBuilder<UrlDbContext>()
                .UseNpgsql(info.ConnectionString)
                .Options;

            _context = new UrlDbContext(options);

            UrlRepo = new GenericRepository<Url, long>(_context);
            UrlAccessLogRepo = new GenericRepository<UrlAccessLog, string>(_context);
        }

        public Task<int> CommitAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);


        public void Dispose()
        {

            _context.Dispose();
        }
    }


    
}
