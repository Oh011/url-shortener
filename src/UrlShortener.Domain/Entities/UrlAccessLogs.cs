namespace Project.Domain.Entities
{
    public class UrlAccessLog
    {
        public long Id { get; set; }         // BIGINT in PostgreSQL, PK
        public long UrlId { get; set; }      // FK to Urls.Id

        public Url Url { get; set; }
        public DateTime AccessedAt { get; set; }
        public string IpAddress { get; set; } = null!;    // IPv6 safe
        public string UserAgent { get; set; } = null!;    // browser/device
        public string? Referrer { get; set; }             // optional
    }

}
