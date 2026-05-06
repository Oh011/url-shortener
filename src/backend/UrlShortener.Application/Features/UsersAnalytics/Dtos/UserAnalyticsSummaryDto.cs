namespace UrlShortener.Application.Features.UsersAnalytics.Dtos
{
    public class UserAnalyticsSummaryDto : UserStatisticsDto
    {




        public int ActiveUrls { get; set; }

        public int ExpiredUrls { get; set; }
    }
}
