namespace Project.Domain.Entities
{
    public class UserAnalytics
    {
        public string UserId { get; set; } = null!;
        public string ShortUrl { get; set; } = null!;
        public string OriginalUrl { get; set; } = null!;

        public DateTime? ExpiresAt { get; set; }

        public long ClickCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastAccessedAt { get; set; } = DateTime.UtcNow;
    }
}
