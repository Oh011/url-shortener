using Project.Application.Features.Analytics.Dtos;

namespace Project.Application.Common.Interfaces.Services
{
    public interface IUserAnalyticsService
    {
        Task DeleteAnalyticsRecordAsync(string userId, string shortUrl, CancellationToken cancellationToken = default);


        Task UpdateAnalyticsRecordAsync(UserAnalyticsDto userAnalyticsDto, CancellationToken cancellationToken = default);

        Task CreateAnalyticsRecordAsync(UserAnalyticsDto dto, CancellationToken cancellationToken = default);
        Task IncrementClickCountAsync(string shortUrl, CancellationToken cancellationToken = default);
    }
}
