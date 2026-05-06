namespace UrlShortener.Application.Features.UsersAnalytics.Dtos
{
    public class UserAnalyticsDto
    {

        public string UserId { get; set; } = null!;
        public string ShortUrl { get; set; } = null!;
        public string OriginalUrl { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}
