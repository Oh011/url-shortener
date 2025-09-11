namespace Project.Domain.Entities
{
    public class Url
    {
        public long Id { get; set; }           // use long / Int64
        public string ShortUrl { get; set; }  // varchar(10)
        public string OriginalUrl { get; set; }
        public long ClickCount { get; set; } = 0;
        public string? UserId { get; set; }    // nullable
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; } =
        DateTime.UtcNow.AddYears(1);


        public ICollection<UrlAccessLog>? urlAccessLogs { get; set; }
    }

}
