using Project.Domain.Common;

namespace Project.Domain.DomainEvents
{
    public record UrlAccessedEvent(
    long UrlId,
      string ShortUrl,

      string IpAddress,
      string UserAgent,
      string? Referrer,
      string? UserID,
      DateTime OccurredOn = default
  ) : IDomainEvent
    {
        public UrlAccessedEvent(long UrlId, string ShortUrl, string ipAddress, string userAgent, string? referrer, string UserId)
            : this(UrlId, ShortUrl, ipAddress, userAgent, referrer, UserId, DateTime.UtcNow)
        {
        }
    }

}
