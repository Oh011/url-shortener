using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Abstrcations.Persistence
{

    public interface IAppUnitOfWork
    {
        IGenericRepository<UserAnalytics, string> UserAnalyticsRepo { get; }

        IGenericRepository<RefreshToken,int> RefreshTokenRepo { get; }
       
        IGenericRepository<UserStatistics, string> UserStatisticsRepo { get; }
        Task<int> CommitAsync(CancellationToken ct = default);
    }

}
