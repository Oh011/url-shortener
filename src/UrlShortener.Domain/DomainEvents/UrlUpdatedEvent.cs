using Project.Domain.Common;

namespace Project.Domain.DomainEvents
{
    public record UrlUpdatedEvent(string UserId, string ShortUrl, string OriginalUrl, DateTime Expiration
     ) : IDomainEvent
    {
        public DateTime OccurredOn { get; set; } = DateTime.Now;


    }
}
