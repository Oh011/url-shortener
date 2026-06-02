using MediatR;
using Microsoft.Extensions.Options;
using Shared.Options;
using Shared.Utilities;
using UrlShortener.Application.Abstrcations.Identity;
using UrlShortener.Application.Abstrcations.Persistence;
using UrlShortener.Application.Common.Events;
using UrlShortener.Application.Common.Factories.Interfaces;
using UrlShortener.Application.Common.Interfaces;
using UrlShortener.Application.Common.Services.Implementations;
using UrlShortener.Application.Exceptions;
using UrlShortener.Application.Features.Urls.Commands;
using UrlShortener.Application.Features.Urls.Dtos;
using UrlShortener.Domain.DomainEvents;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Features.Urls.Handlers.Commands
{
    internal class CreateShortUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, ShortUrlDto>
    {


        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IUniqueIdGenerator uniqueIdGenerator;
        private readonly IShortCodeGenerator shortCodeGenerator;
        private readonly IOptions<ShortenerOptions> options;
        private readonly IMediator _mediator;
        private readonly IUserContextService userContextService;



        public CreateShortUrlCommandHandler(IShortCodeGenerator shortCodeGenerator, IUserContextService userContextService, IMediator mediator, IOptions<ShortenerOptions> options, IUnitOfWorkFactory unitOfWorkFactory, IUniqueIdGenerator uniqueIdGenerator)
        {
            this.userContextService = userContextService;
            this.uniqueIdGenerator = uniqueIdGenerator;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.shortCodeGenerator = shortCodeGenerator;
            this._mediator = mediator;

            this.options = options;
        }


        public async Task<ShortUrlDto> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
        {


            //long nextId = await uniqueIdGenerator.GetNextId();

            var userId = userContextService.GetCurrentUserId();

            var url = new Url
            {
        
                OriginalUrl = request.LongUrl,
                UserId = userId,


            };

            if (request.ExpirationDate.HasValue)
                url.ExpiresAt = request.ExpirationDate.Value;

            var shortenerOptions = options.Value;



            if (request.CustomAlias != null)
            {

                var unitOfWork = unitOfWorkFactory.CreateShardUnitOfWork(request.CustomAlias);

                var repository = unitOfWork.UrlRepo;

                var exists = await repository.ExistsAsync(u => u.ShortUrl == request.CustomAlias);


                if (exists)
                {
                    throw new BadRequestException("Custom alias already exists");
                }


                url.ShortUrl = request.CustomAlias;

                await repository.AddAsync(url);

                await unitOfWork.CommitAsync(cancellationToken);
            }


            else
            {

                string shortCode;
                IGenericRepository<Url,long> repository;

                while (true)
                {
                    shortCode = shortCodeGenerator.Generate();

                    var unitOfWork = unitOfWorkFactory.CreateShardUnitOfWork(shortCode);
                    repository = unitOfWork.UrlRepo;

                    var exists = await repository.ExistsAsync(u => u.ShortUrl == shortCode);

                    if (!exists)
                    {
                        url.ShortUrl = shortCode;
                        await repository.AddAsync(url);
                        await unitOfWork.CommitAsync(cancellationToken);
                        break;
                    }
                }



            }

            await _mediator.Publish(new DomainEventNotifications<UrlCreatedEvent>
                (new UrlCreatedEvent(url.Id, userId, url.ShortUrl, url.OriginalUrl, url.ExpiresAt))

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
