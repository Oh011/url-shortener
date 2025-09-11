using MediatR;
using Project.Application.Common.Events;
using Project.Application.Common.Factories.Interfaces;
using Project.Application.Common.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Application.Exceptions;
using Project.Application.Features.Urls.Dtos;
using Project.Domain.DomainEvents;
using Project.Domain.Entities;

namespace Project.Application.Features.Urls.Commands.UpdateUrl
{
    internal class UpdateUrlCommandHandler : IRequestHandler<UpdateUrlCommand, UrlDetailsDto>
    {

        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IUserContextService userContextService;
        private readonly ICacheRepository cacheRepository;
        private readonly IMediator mediator;


        public UpdateUrlCommandHandler(IMediator mediator, ICacheRepository cacheRepository, IUnitOfWorkFactory unitOfWorkFactory, IUserContextService userContextService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.userContextService = userContextService;
            this.cacheRepository = cacheRepository;
            this.mediator = mediator;
        }

        public async Task<UrlDetailsDto> Handle(UpdateUrlCommand request, CancellationToken cancellationToken)
        {

            var unitOfWork = unitOfWorkFactory.CreateShardUnitOfWork(request.ShortUrl);
            var repository = unitOfWork.GetRepository<Url, long>();
            var userId = userContextService.GetCurrentUserId();


            var url = await repository.FirstOrDefaultAsync(u => u.ShortUrl == request.ShortUrl
            && u.ExpiresAt > DateTime.UtcNow);


            if (url == null) throw new NotFoundException("Short URL not found");


            if (url.UserId != userId)
                throw new UnAuthorizedException("not Authorized to update Url");


            url.ExpiresAt = request.NewExpiresAt;


            repository.Update(url);

            await cacheRepository.RemoveAsync(url.ShortUrl);

            await unitOfWork.Commit();


            await mediator.Publish(new DomainEventNotifications<UrlUpdatedEvent>(

                new UrlUpdatedEvent(userId, url.ShortUrl, url.OriginalUrl, url.ExpiresAt
                )));




            return new UrlDetailsDto
            {
                ShortUrl = request.ShortUrl,
                LongUrl = url.OriginalUrl,
                ExpirationDate = url.ExpiresAt,
                TotalClicks = url.ClickCount,
                CreatedAt = url.CreatedAt,
            };




        }
    }
}
