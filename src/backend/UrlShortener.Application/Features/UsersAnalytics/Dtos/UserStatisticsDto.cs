namespace UrlShortener.Application.Features.UsersAnalytics.Dtos
{
    public class UserStatisticsDto
    {

        public int TotalUrls { get; set; } = 0;
        public long TotalClicks { get; set; } = 0;
        public DateTime LastActiveAt { get; set; } = DateTime.UtcNow;
    }
}
