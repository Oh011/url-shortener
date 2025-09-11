using MediatR;
using Project.Application.Common.Factories.Interfaces;
using Project.Application.Exceptions;
using Project.Application.Features.Urls.Dtos;
using Project.Domain.Entities;

namespace Project.Application.Features.Urls.Queries.GetUrlDetails
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
            var repository = unitOfWork.GetRepository<Url, long>();

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
