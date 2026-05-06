using MediatR;
using UrlShortener.Application.Common.Factories.Interfaces;
using UrlShortener.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Application.Features.UrlAccessLogs.Commands;
using UrlShortener.Application.Abstrcations.Caching;

namespace UrlShortener.Application.Features.UrlAccessLogs.Handlers.CommandHandlers
{
    internal class LogUrlAccessCommandHandler : IRequestHandler<LogUrlAccessCommand>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ICacheRepository _cacheRepository;

        public LogUrlAccessCommandHandler(
            IUnitOfWorkFactory unitOfWorkFactory,
            ICacheRepository cacheRepository)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _cacheRepository = cacheRepository;
        }

        public async Task Handle(LogUrlAccessCommand request, CancellationToken cancellationToken)
        {
            // pick the correct shard based on ShortUrl
            var uow = _unitOfWorkFactory.CreateShardUnitOfWork(request.ShortUrl);

            var urlsRepo = uow.UrlRepo;
            var logsRepo = uow.UrlAccessLogRepo;

            // get the URL entity
            var url = await urlsRepo.GetById(request.UrlId);

            if (url == null)
                return; // nothing to do

            // increment click count
            url.ClickCount += 1;

            // create access log
            var log = new UrlAccessLog
            {
                UrlId = request.UrlId,
                IpAddress = request.IpAddress,
                Referrer = request.Referrer,
                UserAgent = request.UserAgent,
                AccessedAt = DateTime.UtcNow
            };

            await logsRepo.AddAsync(log);

            // remove cache key (for eventual consistency)
            try
            {
                await _cacheRepository.RemoveAsync($"clicks:{request.ShortUrl}");
                await uow.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // optionally log exception, do NOT throw to crash background job
                Console.WriteLine(ex.Message);
            }
        }
    }

}
