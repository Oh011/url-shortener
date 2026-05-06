using MediatR;
using UrlShortener.Application.Abstrcations.BackGround;
using UrlShortener.Application.Common.Events;
using UrlShortener.Application.Features.UrlAccessLogs.Commands;
using UrlShortener.Application.Features.UrlAccessLogs.Dtos;
using UrlShortener.Application.Features.UsersAnalytics.Commands;
using UrlShortener.Application.Features.UserStatistcis.commands;
using UrlShortener.Domain.DomainEvents;

namespace UrlShortener.Application.Features.UrlAccessLogs.EventHandlers
{
    internal class UrlAccessedEventHandler : INotificationHandler<DomainEventNotifications<UrlAccessedEvent>>

    {

        private readonly IBackgroundJobService backgroundJobService;



        public UrlAccessedEventHandler(IBackgroundJobService backgroundJobService)
        {
            this.backgroundJobService = backgroundJobService;
          
        }
        public Task Handle(DomainEventNotifications<UrlAccessedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            var userId = domainEvent.UserID;


            if (userId != null)
                backgroundJobService.Enqueue<IMediator>(us => us.Send(new IncrementTotalClicksCommand (userId, 1), cancellationToken));



            var command = new LogUrlAccessCommand(


                domainEvent.UrlId, domainEvent.ShortUrl, domainEvent.IpAddress, domainEvent.Referrer, domainEvent.UserAgent

                );

           


            backgroundJobService.Enqueue<IMediator>(m=>m.Send(new IncrementClickCommand(domainEvent.ShortUrl), cancellationToken));


            backgroundJobService.Enqueue<IMediator>(m => m.Send(command,cancellationToken));

            return Task.CompletedTask;
        }
    }
}
