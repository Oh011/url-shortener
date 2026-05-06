using MediatR;
using UrlShortener.Application.Common.Factories.Interfaces;
using UrlShortener.Application.Features.Urls.Dtos;
using UrlShortener.Application.Features.Urls.Specifications;
using UrlShortener.Domain.Entities;
using Shared.Dtos;
using UrlShortener.Application.Features.Urls.Queries;
using UrlShortener.Application.Abstrcations.Identity;

namespace UrlShortener.Application.Features.Urls.Handlers.QueryHandlers
{
    internal class GetUserUrlsQueryHandler : IRequestHandler<GetUserUrlsQuery, PaginatedResult<UrlDetailsDto>>
    {

        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IUserContextService _userContextService;

        public GetUserUrlsQueryHandler(IUserContextService userContextService, IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this._userContextService = userContextService;
        }

        public async Task<PaginatedResult<UrlDetailsDto>> Handle(GetUserUrlsQuery request, CancellationToken cancellationToken)
        {


            var userId = _userContextService.GetCurrentUserId();
            var appUnitOfWork = unitOfWorkFactory.CreateAppDbUnitOfWork();


            var repository = appUnitOfWork.UserAnalyticsRepo;


            var specifications = new UserUrlsSpecifications(request, userId);


            var urls = (await repository.GetAllWithSpecifications(specifications))
                .Select(x => new UrlDetailsDto
                {
                    ShortUrl = x.ShortUrl,
                    CreatedAt = x.CreatedAt,
                    LongUrl = x.OriginalUrl,
                    ExpirationDate = x.ExpiresAt,
                    TotalClicks = x.ClickCount,
                });



            var totalCount = await repository.CountAsync(specifications.Criteria);



            return new PaginatedResult<UrlDetailsDto>(request.PageIndex, request.pageSize,
                totalCount, urls);






        }




    }
}
