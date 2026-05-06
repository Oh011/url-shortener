using UrlShortener.Domain.Common;

namespace UrlShortener.Domain.DomainEvents
{
    public record UrlDeletedEvent(string UserId, string ShortUrl
     ) : IDomainEvent
    {
        public DateTime OccurredOn { get; set; } = DateTime.Now;
    }
}
