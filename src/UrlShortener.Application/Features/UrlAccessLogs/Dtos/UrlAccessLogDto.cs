namespace Project.Application.Features.UrlAccessLogs.Dtos
{
    public class UrlAccessLogDto
    {

        public long UrlId { get; set; }
        public string ShortUrl { get; set; }           // FK to Url

        public string IpAddress { get; set; } = null!;
        public string UserAgent { get; set; } = null!;
        public string? Referrer { get; set; }
        public DateTime AccessedAt { get; set; } = DateTime.UtcNow;
    }

}
