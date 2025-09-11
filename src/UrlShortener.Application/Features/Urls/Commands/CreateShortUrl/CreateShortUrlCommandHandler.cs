using MediatR;
using Microsoft.Extensions.Options;
using Project.Application.Common.Events;
using Project.Application.Common.Factories.Interfaces;
using Project.Application.Common.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Application.Exceptions;
using Project.Application.Features.Urls.Dtos;
using Project.Domain.DomainEvents;
using Project.Domain.Entities;
using Shared.Options;
using Shared.Utilities;

namespace Project.Application.Features.Urls.Commands.CreateShortUrl
{
    internal class CreateShortUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, ShortUrlDto>
    {


        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IUniqueIdGenerator uniqueIdGenerator;
        private readonly IOptions<ShortenerOptions> options;
        private readonly IMediator _mediator;
        private readonly IUserContextService userContextService;



        public CreateShortUrlCommandHandler(IUserContextService userContextService, IMediator mediator, IOptions<ShortenerOptions> options, IUnitOfWorkFactory unitOfWorkFactory, IUniqueIdGenerator uniqueIdGenerator)
        {
            this.userContextService = userContextService;
            this.uniqueIdGenerator = uniqueIdGenerator;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this._mediator = mediator;

            this.options = options;
        }


        public async Task<ShortUrlDto> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
        {


            long nextId = await uniqueIdGenerator.GetNextId();

            var userId = userContextService.GetCurrentUserId();

            var url = new Url
            {
                Id = nextId,
                OriginalUrl = request.LongUrl,
                UserId = userId,


            };

            if (request.ExpirationDate.HasValue)
                url.ExpiresAt = request.ExpirationDate.Value;

            var shortenerOptions = options.Value;



            if (request.CustomAlias != null)
            {

                var unitOfWork = unitOfWorkFactory.CreateShardUnitOfWork(request.CustomAlias);

                var repository = unitOfWork.GetRepository<Url, long>();

                var exists = await repository.ExistsAsync(u => u.ShortUrl == request.CustomAlias);


                if (exists)
                {
                    throw new BadRequestException("Custom alias already exists");
                }


                url.ShortUrl = request.CustomAlias;



                await repository.AddAsync(url);

                await unitOfWork.Commit(cancellationToken);
            }


            else
            {

                var shortCode = Base62.Encode(nextId ^ shortenerOptions.SecretKey);
                var unitOfWork = unitOfWorkFactory.CreateShardUnitOfWork(shortCode);
                var repository = unitOfWork.GetRepository<Url, long>();

                url.ShortUrl = shortCode;


                await repository.AddAsync(url);

                await unitOfWork.Commit(cancellationToken);



            }

            await _mediator.Publish(new DomainEventNotifications<UrlCreatedEvent>
                (new UrlCreatedEvent(nextId, userId, url.ShortUrl, url.OriginalUrl, url.ExpiresAt))

                );


            return new ShortUrlDto
            {

                ShortUrl = $"{options.Value.BaseUrl}/{url.ShortUrl}",
                LongUrl = request.LongUrl,
                ExpirationDate = url.ExpiresAt,
                CustomAlias = request.CustomAlias,
                CreatedAt = url.CreatedAt,
            };


        }
    }
}
