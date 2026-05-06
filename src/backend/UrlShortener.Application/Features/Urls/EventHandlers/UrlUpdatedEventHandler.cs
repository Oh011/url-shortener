using MediatR;
using UrlShortener.Application.Common.Events;
using UrlShortener.Domain.DomainEvents;
using UrlShortener.Application.Features.UsersAnalytics.Commands;
using UrlShortener.Application.Abstrcations.BackGround;

namespace UrlShortener.Application.Features.Urls.EventHandlers
{
    internal class UrlUpdatedEventHandler : INotificationHandler<DomainEventNotifications<UrlUpdatedEvent>>
    {


        private readonly IBackgroundJobService backgroundJobService;
        private readonly IMediator mediator;


        public UrlUpdatedEventHandler(IBackgroundJobService backgroundJobService,IMediator mediator)
        {

            this.backgroundJobService = backgroundJobService;
            this.mediator = mediator;
        }
        public Task Handle(DomainEventNotifications<UrlUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;


            var command = new UpdateAnalyticsCommand
            (

                domainEvent.UserId,
                domainEvent.ShortUrl,
                 domainEvent.OriginalUrl,
                domainEvent.Expiration
            );



            backgroundJobService.Enqueue<IMediator>(s=>s.Send(command,cancellationToken));


          

            return Task.CompletedTask;
        }
    }


}
