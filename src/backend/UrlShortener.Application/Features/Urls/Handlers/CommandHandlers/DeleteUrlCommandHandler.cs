using MediatR;
using UrlShortener.Application.Common.Events;
using UrlShortener.Application.Common.Factories.Interfaces;
using UrlShortener.Application.Exceptions;
using UrlShortener.Domain.DomainEvents;
using UrlShortener.Domain.Entities;
using UrlShortener.Application.Features.Urls.Commands;
using UrlShortener.Application.Abstrcations.Identity;
using UrlShortener.Application.Abstrcations.Caching;

namespace UrlShortener.Application.Features.Urls.Handlers.Commands
{
    internal class DeleteUrlCommandHandler : IRequestHandler<DeleteUrlCommand>
    {

        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IUserContextService userContextService;
        private readonly ICacheRepository cacheRepository;
        private readonly IMediator mediator;


        public DeleteUrlCommandHandler(IMediator mediator, ICacheRepository cacheRepository, IUnitOfWorkFactory unitOfWorkFactory, IUserContextService userContextService)
        {

            this.mediator = mediator;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.userContextService = userContextService;
            this.cacheRepository = cacheRepository;
        }
        public async Task Handle(DeleteUrlCommand request, CancellationToken cancellationToken)
        {



            var unitOfWork = unitOfWorkFactory.CreateShardUnitOfWork(request.ShortUrl);


            var repository = unitOfWork.UrlRepo;

            var userId = userContextService.GetCurrentUserId();


            var url = await repository.FirstOrDefaultAsync(u => u.ShortUrl == request.ShortUrl
            && u.ExpiresAt > DateTime.UtcNow);




            if (url == null) throw new NotFoundException("Short URL not found");


            if (url.UserId != userId)
                throw new UnAuthorizedException("not Authorized to update Url");





            await cacheRepository.RemoveAsync(url.ShortUrl);

            repository.Delete(url);



            await unitOfWork.CommitAsync();



            await mediator.Publish(new DomainEventNotifications<UrlDeletedEvent>(
                  new UrlDeletedEvent(userId, request.ShortUrl)), cancellationToken);


        }
    }
}
