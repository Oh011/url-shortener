using MediatR;
using Project.Application.Common.Events;
using Project.Application.Common.Factories.Interfaces;
using Project.Application.Common.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Application.Exceptions;
using Project.Domain.DomainEvents;
using Project.Domain.Entities;

namespace Project.Application.Features.Urls.Commands.DeleteUrl
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


            var repository = unitOfWork.GetRepository<Url, long>();

            var userId = userContextService.GetCurrentUserId();


            var url = await repository.FirstOrDefaultAsync(u => u.ShortUrl == request.ShortUrl
            && u.ExpiresAt > DateTime.UtcNow);




            if (url == null) throw new NotFoundException("Short URL not found");


            if (url.UserId != userId)
                throw new UnAuthorizedException("not Authorized to update Url");







            await cacheRepository.RemoveAsync(url.ShortUrl);

            repository.Delete(url);



            await unitOfWork.Commit();



            await mediator.Publish(new DomainEventNotifications<UrlDeletedEvent>(
                  new UrlDeletedEvent(userId, request.ShortUrl)), cancellationToken);


        }
    }
}
