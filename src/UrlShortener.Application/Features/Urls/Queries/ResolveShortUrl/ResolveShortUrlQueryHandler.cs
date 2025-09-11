using MediatR;
using Project.Application.Common.Events;
using Project.Application.Common.Factories.Interfaces;
using Project.Application.Common.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Application.Exceptions;
using Project.Application.Features.Urls.Dtos;
using Project.Domain.DomainEvents;
using Project.Domain.Entities;

namespace Project.Application.Features.Urls.Queries.ResolveShortUrl
{
    internal class ResolveShortUrlQueryHandler : IRequestHandler<ResolveShortUrlQuery, string>
    {

        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IMediator _mediator;
        private readonly ICacheRepository _cacheRepository;
        private readonly IUserContextService userContextService;


        public ResolveShortUrlQueryHandler(IUserContextService userContextService, ICacheRepository cacheRepository, IUnitOfWorkFactory unitOfWorkFactory, IMediator mediator)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            _mediator = mediator;
            _cacheRepository = cacheRepository;
            this.userContextService = userContextService;
        }

        public async Task<string> Handle(ResolveShortUrlQuery request, CancellationToken cancellationToken)
        {


            var unitOfWork = unitOfWorkFactory.CreateShardUnitOfWork(request.ShortUrl);
            var repository = unitOfWork.GetRepository<Url, long>();


            var urlInfo = await _cacheRepository.GetAsync<UrlInfoDto>(request.ShortUrl);

            if (urlInfo == null)
            {

                urlInfo = await repository.FirstOrDefaultAsync(u => u.ShortUrl == request.ShortUrl
                && u.ExpiresAt > DateTime.UtcNow,
                      u => new UrlInfoDto
                      {
                          LongUrl = u.OriginalUrl,
                          ShortUrl = u.ShortUrl,
                          UrlId = u.Id,

                      });

                if (urlInfo == null)
                {

                    throw new NotFoundException("Short URL not found");
                }

                await _cacheRepository.SetAsync<UrlInfoDto>(request.ShortUrl, urlInfo, TimeSpan.FromHours(1));
            }





            var ipAddress = request.IpAddress;
            var userAgent = request.UserAgent;
            var referrer = request.Referrer;


            await _mediator.Publish(new DomainEventNotifications<UrlAccessedEvent>
                (new UrlAccessedEvent(urlInfo.UrlId, request.ShortUrl, ipAddress, userAgent, referrer, userContextService.GetCurrentUserId())),
                cancellationToken
            );


            return urlInfo.LongUrl;
        }
    }
}
