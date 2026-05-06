using MediatR;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Common.Factories.Interfaces;
using UrlShortener.Application.Features.UsersAnalytics.Dtos;
using UrlShortener.Domain.Entities;
using Shared.Options;
using UrlShortener.Application.Features.UsersAnalytics.queries;
using UrlShortener.Application.Abstrcations.Identity;

namespace UrlShortener.Application.Features.UsersAnalytics.Handlers.QueryHandlers
{
    internal class GetTopUrlsQueryHandler : IRequestHandler<GetTopUserUrlsQuery, IEnumerable<TopUrlDto>>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IUserContextService _userContextService;
        private readonly IOptions<ShortenerOptions> options;

        public GetTopUrlsQueryHandler(IOptions<ShortenerOptions> options, IUnitOfWorkFactory unitOfWorkFactory, IUserContextService userContextService)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _userContextService = userContextService;
            this.options = options;
        }

        public async Task<IEnumerable<TopUrlDto>> Handle(GetTopUserUrlsQuery request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetCurrentUserId();
            var appUnitOfWork = _unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repo = appUnitOfWork.UserAnalyticsRepo;


            var topUrls = await repo.GetFilteredPaginatedAsync(
                predicate: u => u.UserId == userId,
                sort: u => u.ClickCount,
                selector: u => new TopUrlDto
                {
                    ShortUrl = $"{options.Value.BaseUrl}/{u.ShortUrl}",
                    OriginalUrl = u.OriginalUrl,
                    Clicks = u.ClickCount,
                    CreatedAt = u.CreatedAt
                },
                take: request.Count,
                skip: null,
                ascending: false,
                cancellationToken: cancellationToken
                    );







            return topUrls;
        }
    }

}
