using MediatR;
using UrlShortener.Application.Common.Factories.Interfaces;
using UrlShortener.Application.Features.UsersAnalytics.Dtos;
using UrlShortener.Domain.Entities;
using UrlShortener.Application.Features.UsersAnalytics.queries;
using UrlShortener.Application.Abstrcations.Identity;

namespace UrlShortener.Application.Features.UsersAnalytics.Handlers.QueryHandlers
{
    internal class GetUserAnalyticsSummaryQueryHandler : IRequestHandler<GetUserAnalyticsSummaryQuery, UserAnalyticsSummaryDto>
    {


        private readonly IUserContextService _userContextService;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;


        public GetUserAnalyticsSummaryQueryHandler(IUserContextService userContextService, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _userContextService = userContextService;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<UserAnalyticsSummaryDto> Handle(GetUserAnalyticsSummaryQuery request, CancellationToken cancellationToken)
        {
            var userId = _userContextService.GetCurrentUserId();


            var appUnitOfWork = unitOfWorkFactory.CreateAppDbUnitOfWork();
            var userStatisticsRepository = appUnitOfWork.UserStatisticsRepo;

            var userAnalyticsRepository = appUnitOfWork.UserAnalyticsRepo;



            var userStatistics = await userStatisticsRepository.FirstOrDefaultAsync(
               s => s.UserId == userId, s => new UserStatisticsDto
               {
                   TotalClicks = s.TotalClicks,
                   TotalUrls = s.TotalUrls,
                   LastActiveAt = s.LastActiveAt,
               }
                );


            var totalActiveUrls = await userAnalyticsRepository.CountAsync(

                u => u.UserId == userId && u.ExpiresAt > DateTime.UtcNow

                );

            var totalExpiredUrls = userStatistics.TotalUrls - totalActiveUrls;




            return new UserAnalyticsSummaryDto
            {

                TotalUrls = userStatistics.TotalUrls,
                TotalClicks = userStatistics.TotalClicks,
                LastActiveAt = userStatistics.LastActiveAt,
                ActiveUrls = totalActiveUrls,
                ExpiredUrls = totalExpiredUrls
            };

        }
    }
}
