using MediatR;
using Project.Application.Common.Factories.Interfaces;
using Project.Application.Common.Interfaces;
using Project.Application.Exceptions;
using Project.Application.Features.Urls.Dtos;
using Project.Domain.Entities;

namespace Project.Application.Features.Urls.Queries.GetUrlClicksCount
{
    internal class GetUrlClicksCountQueryHandler : IRequestHandler<GetUrlClicksCountQuery, UrlClickCountsDto>
    {

        private readonly ICacheRepository _cacheRepository;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;


        public GetUrlClicksCountQueryHandler(ICacheRepository repository, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _cacheRepository = repository;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }
        public async Task<UrlClickCountsDto> Handle(GetUrlClicksCountQuery request, CancellationToken cancellationToken)
        {


            var cacheKey = $"clicks:{request.ShortUrl}";

            // 1. Try Redis
            var cachedCount = await _cacheRepository.GetAsync<int>(cacheKey);
            if (cachedCount != default)
            {
                return new UrlClickCountsDto
                {
                    ShortUrl = request.ShortUrl,
                    Count = cachedCount

                };
            }

            // 2. Fallback to DB
            var unitOfWork = unitOfWorkFactory.CreateShardUnitOfWork(request.ShortUrl);
            var repository = unitOfWork.GetRepository<Url, long>();

            var url = await repository.FirstOrDefaultAsync(
                u => u.ShortUrl == request.ShortUrl,
                u => new UrlClickCountsDto
                {
                    ShortUrl = u.ShortUrl,
                    Count = u.ClickCount

                });

            if (url == null)
            {
                throw new NotFoundException("Short URL not found");
            }


            // 3. Cache the result with TTL
            await _cacheRepository.SetAsync(cacheKey, url.Count, TimeSpan.FromMinutes(5));

            return url;
        }
    }
}
