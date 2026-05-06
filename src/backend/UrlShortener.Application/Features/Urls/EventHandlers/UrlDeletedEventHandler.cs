using MediatR;
using UrlShortener.Application.Common.Events;
using UrlShortener.Domain.DomainEvents;
using UrlShortener.Application.Features.UsersAnalytics.Commands;
using UrlShortener.Application.Features.UserStatistcis.commands;
using UrlShortener.Application.Abstrcations.BackGround;

namespace UrlShortener.Application.Features.Urls.EventHandlers
{
    internal class UrlDeletedEventHandler : INotificationHandler<DomainEventNotifications<UrlDeletedEvent>>
    {


        private readonly IBackgroundJobService backgroundJobService;
        private readonly IMediator mediator;    


        public UrlDeletedEventHandler(IBackgroundJobService backgroundJobService, IMediator mediator)
        {

            this.backgroundJobService = backgroundJobService;
            this.mediator = mediator;
        }
        public Task Handle(DomainEventNotifications<UrlDeletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            backgroundJobService.Enqueue<IMediator>(s => s.Send(new UpdateTotalUrlsCommand(domainEvent.UserId,-1) , cancellationToken));

            backgroundJobService.Enqueue<IMediator>(s => s.Send(new DeleteAnalyticsCommand(domainEvent.UserId, domainEvent.ShortUrl), cancellationToken));


          
            return Task.CompletedTask;
        }
    }
}
