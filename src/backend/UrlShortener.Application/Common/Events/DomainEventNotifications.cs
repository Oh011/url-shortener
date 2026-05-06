using MediatR;
using UrlShortener.Domain.Common;

namespace UrlShortener.Application.Common.Events
{
    public class DomainEventNotifications<TDomainEvent> : INotification
         where TDomainEvent : IDomainEvent
    {

        public TDomainEvent DomainEvent { get; }

        public DomainEventNotifications(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }
    }
}
