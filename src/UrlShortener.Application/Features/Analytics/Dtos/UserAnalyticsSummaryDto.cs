namespace Project.Application.Features.Analytics.Dtos
{
    public class UserAnalyticsSummaryDto : UserStatisticsDto
    {




        public int ActiveUrls { get; set; }

        public int ExpiredUrls { get; set; }
    }
}
