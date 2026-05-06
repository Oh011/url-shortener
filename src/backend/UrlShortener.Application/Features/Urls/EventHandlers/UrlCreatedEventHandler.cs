using MediatR;
using UrlShortener.Application.Common.Events;
using UrlShortener.Domain.DomainEvents;
using UrlShortener.Application.Features.UsersAnalytics.Commands;
using UrlShortener.Application.Features.UserStatistcis.commands;
using UrlShortener.Application.Abstrcations.BackGround;

namespace UrlShortener.Application.Features.Urls.EventHandlers
{
    internal class UrlCreatedEventHandler : INotificationHandler<DomainEventNotifications<UrlCreatedEvent>>
    {


        private readonly IBackgroundJobService backgroundJobService;
        private readonly IMediator mediator;


        public UrlCreatedEventHandler(IBackgroundJobService backgroundJobService, IMediator mediator)
        {

            this.backgroundJobService = backgroundJobService;
            this.mediator = mediator;
        }
        public Task Handle(DomainEventNotifications<UrlCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;


            if (domainEvent.UserId != null)
            {

                backgroundJobService.Enqueue<IMediator>(us => us.Send(new UpdateTotalUrlsCommand(domainEvent.UserId,1) , cancellationToken));

                var userAnalyticsDto = new CreateAnalyticsCommand
                (
                     domainEvent.UserId,
                     domainEvent.OriginalUrl,
                     domainEvent.ShortUrl,
                    domainEvent.ExpiresAt

                );



                backgroundJobService.Enqueue<IMediator>(x => mediator.Send(userAnalyticsDto, cancellationToken));

            }




            return Task.CompletedTask;
        }
    }

}
