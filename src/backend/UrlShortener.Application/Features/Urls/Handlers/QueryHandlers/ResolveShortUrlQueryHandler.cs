using MediatR;
using UrlShortener.Application.Common.Events;
using UrlShortener.Application.Common.Factories.Interfaces;
using UrlShortener.Application.Exceptions;
using UrlShortener.Application.Features.Urls.Dtos;
using UrlShortener.Domain.DomainEvents;
using UrlShortener.Domain.Entities;
using UrlShortener.Application.Features.Urls.Queries;
using UrlShortener.Application.Abstrcations.Identity;
using UrlShortener.Application.Abstrcations.Caching;

namespace UrlShortener.Application.Features.Urls.Handlers.QueryHandlers
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
            var repository = unitOfWork.UrlRepo;


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
