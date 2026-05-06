using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Persistence.Context;
using System.Collections.Concurrent;
using UrlShortener.Infrastructure.Identity.Entities;
using UrlShortener.Application.Abstrcations.Persistence;

namespace UrlShortener.Infrastructure.Persistence.Repositories
{
    internal class AppUnitOfWork : IAppUnitOfWork
    {


        private readonly ApplicationDbContext _context;

        public IGenericRepository<UserAnalytics, string> UserAnalyticsRepo { get; }

        public IGenericRepository<UserStatistics, string> UserStatisticsRepo { get; }

        public IGenericRepository<RefreshToken, int> RefreshTokenRepo { get; }

        public AppUnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            UserAnalyticsRepo = new GenericRepository<UserAnalytics, string>(_context);
            UserStatisticsRepo = new GenericRepository<UserStatistics, string>(_context);
            RefreshTokenRepo=new GenericRepository<RefreshToken, int>(_context);
        }

        public Task<int> CommitAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);


      

        public void Dispose()
        {

            _context.Dispose();
        }

     
    }
}
