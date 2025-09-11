using MediatR;
using Project.Application.Common.Factories.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Application.Features.Urls.Dtos;
using Project.Application.Features.Urls.Specifications;
using Project.Domain.Entities;
using Shared.Dtos;

namespace Project.Application.Features.Urls.Queries.GetUserUrls
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


            var repository = appUnitOfWork.GetRepository<UserAnalytics, string>();


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
