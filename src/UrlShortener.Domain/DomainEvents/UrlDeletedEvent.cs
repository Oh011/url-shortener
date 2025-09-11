using Project.Domain.Common;

namespace Project.Domain.DomainEvents
{
    public record UrlDeletedEvent(string UserId, string ShortUrl
     ) : IDomainEvent
    {
        public DateTime OccurredOn { get; set; } = DateTime.Now;
    }
}
