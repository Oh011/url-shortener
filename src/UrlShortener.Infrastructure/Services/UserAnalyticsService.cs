using Project.Application.Common.Factories.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Application.Features.Analytics.Dtos;
using Project.Domain.Entities;

namespace Project.Infrastructure.Services
{
    internal class UserAnalyticsService : IUserAnalyticsService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UserAnalyticsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task CreateAnalyticsRecordAsync(UserAnalyticsDto dto, CancellationToken cancellationToken = default)
        {
            var appUnitOfWork = _unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repo = appUnitOfWork.GetRepository<UserAnalytics, string>();

            var existing = await repo.ExistsAsync(
                a => a.UserId == dto.UserId && a.ShortUrl == dto.ShortUrl
            );

            if (!existing)
            {
                var record = new UserAnalytics
                {
                    UserId = dto.UserId,
                    ShortUrl = dto.ShortUrl,
                    OriginalUrl = dto.OriginalUrl,
                    ClickCount = 0,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = dto.ExpiresAt,
                    LastAccessedAt = null,

                };

                await repo.AddAsync(record);
                await appUnitOfWork.Commit(cancellationToken);
            }
        }

        public async Task IncrementClickCountAsync(string shortUrl, CancellationToken cancellationToken = default)
        {
            var appUnitOfWork = _unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repo = appUnitOfWork.GetRepository<UserAnalytics, string>();

            var record = await repo.FirstOrDefaultAsync(a => a.ShortUrl == shortUrl);

            if (record != null)
            {
                record.ClickCount += 1;
                record.LastAccessedAt = DateTime.UtcNow;

                await appUnitOfWork.Commit(cancellationToken);
            }
        }


        public async Task DeleteAnalyticsRecordAsync(string userId, string shortUrl, CancellationToken cancellationToken = default)
        {
            var appUnitOfWork = _unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repo = appUnitOfWork.GetRepository<UserAnalytics, string>();

            var record = await repo.FirstOrDefaultAsync(a => a.UserId == userId && a.ShortUrl == shortUrl);

            if (record != null)
            {
                repo.Delete(record);
                await appUnitOfWork.Commit(cancellationToken);
            }
        }

        public async Task UpdateAnalyticsRecordAsync(UserAnalyticsDto userAnalyticsDto, CancellationToken cancellationToken = default)
        {

            var appUnitOfWork = _unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repo = appUnitOfWork.GetRepository<UserAnalytics, string>();

            var record = await repo.FirstOrDefaultAsync(a => a.UserId == userAnalyticsDto.UserId && a.ShortUrl == userAnalyticsDto.ShortUrl);

            if (record != null)
            {
                record.ShortUrl = userAnalyticsDto.ShortUrl;
                record.UserId = userAnalyticsDto.UserId;
                record.OriginalUrl = userAnalyticsDto.OriginalUrl;
                record.ExpiresAt = userAnalyticsDto.ExpiresAt;

                await appUnitOfWork.Commit(cancellationToken);
            }
        }
    }

}
