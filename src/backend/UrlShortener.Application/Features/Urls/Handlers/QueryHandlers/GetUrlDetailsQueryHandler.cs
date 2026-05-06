using MediatR;
using UrlShortener.Application.Common.Factories.Interfaces;
using UrlShortener.Application.Exceptions;
using UrlShortener.Application.Features.Urls.Dtos;
using UrlShortener.Domain.Entities;
using UrlShortener.Application.Features.Urls.Queries;

namespace UrlShortener.Application.Features.Urls.Handlers.QueryHandlers
{
    internal class GetUrlDetailsQueryHandler : IRequestHandler<GetUrlDetailsQuery, UrlDetailsDto>
    {

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;



        public GetUrlDetailsQueryHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<UrlDetailsDto> Handle(GetUrlDetailsQuery request, CancellationToken cancellationToken)
        {

            var unitOfWork = _unitOfWorkFactory.CreateShardUnitOfWork(request.ShortUrl);
            var repository = unitOfWork.UrlRepo;

            var url = await repository.FirstOrDefaultAsync(u => u.ShortUrl == request.ShortUrl,
                u => new UrlDetailsDto
                {

                    ShortUrl = u.ShortUrl,
                    LongUrl = u.OriginalUrl,
                    ExpirationDate = u.ExpiresAt,
                    CreatedAt = u.CreatedAt,
                    TotalClicks = u.ClickCount,
                });


            if (url == null)
                throw new NotFoundException("Url is not found");



            return url;
        }
    }
}
