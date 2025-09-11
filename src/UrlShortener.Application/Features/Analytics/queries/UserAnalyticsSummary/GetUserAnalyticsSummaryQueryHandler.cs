using MediatR;
using Project.Application.Common.Factories.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Application.Features.Analytics.Dtos;
using Project.Domain.Entities;

namespace Project.Application.Features.Analytics.queries.UserAnalyticsSummary
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
            var userStatisticsRepository = appUnitOfWork.GetRepository<UserStatistics, string>();

            var userAnalyticsRepository = appUnitOfWork.GetRepository<UserAnalytics, string>();



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
