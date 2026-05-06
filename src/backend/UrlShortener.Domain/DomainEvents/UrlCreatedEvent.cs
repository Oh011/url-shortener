using UrlShortener.Domain.Common;

namespace UrlShortener.Domain.DomainEvents
{
    public record UrlCreatedEvent(long UrlId, string? UserId, string ShortUrl
    , string OriginalUrl, DateTime ExpiresAt) : IDomainEvent
    {
        public DateTime OccurredOn { get; set; } = DateTime.Now;
    }
}
