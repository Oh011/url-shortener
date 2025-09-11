using Project.Application.Common.Factories.Interfaces;
using Project.Application.Common.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Application.Features.UrlAccessLogs.Dtos;
using Project.Domain.Entities;

namespace Project.Infrastructure.Services
{
    internal class UrlAccessLogService : IUrlAccessLogService
    {

        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly ICacheRepository cacheRepository;


        public UrlAccessLogService(IUnitOfWorkFactory unitOfWorkFactory, ICacheRepository cacheRepository)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.cacheRepository = cacheRepository;
        }

        public async Task LogAccessAsync(UrlAccessLogDto logDto, CancellationToken cancellationToken = default)
        {

            var unitOfWork = unitOfWorkFactory.CreateShardUnitOfWork(logDto.ShortUrl);
            var UrlsRepository = unitOfWork.GetRepository<Url, long>();
            var UrlsAccessLogsRepository = unitOfWork.GetRepository<UrlAccessLog, long>();

            var url = await UrlsRepository.GetById(logDto.UrlId);


            if (url != null)
            {

                url.ClickCount += 1;
                var log = new UrlAccessLog
                {
                    UrlId = logDto.UrlId,
                    IpAddress = logDto.IpAddress,
                    Referrer = logDto.Referrer,
                    UserAgent = logDto.UserAgent,
                    AccessedAt = DateTime.UtcNow
                };

                await UrlsAccessLogsRepository.AddAsync(log);


            }


            try
            {
                await cacheRepository.RemoveAsync($"clicks:{logDto.ShortUrl}");

                await unitOfWork.Commit(cancellationToken);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
