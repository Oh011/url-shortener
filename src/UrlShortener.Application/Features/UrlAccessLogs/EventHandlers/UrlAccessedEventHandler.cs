using MediatR;
using Project.Application.Common.Events;
using Project.Application.Common.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Application.Features.UrlAccessLogs.Dtos;
using Project.Domain.DomainEvents;

namespace Project.Application.Features.UrlAccessLogs.EventHandlers
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
                backgroundJobService.Enqueue<IUserStatisticsService>(us => us.IncrementTotalClicksAsync(userId, 1, cancellationToken));


            var accessDto = new UrlAccessLogDto
            {
                Referrer = domainEvent.Referrer,
                IpAddress = domainEvent.IpAddress,
                ShortUrl = domainEvent.ShortUrl,
                UserAgent = domainEvent.UserAgent,
                UrlId = domainEvent.UrlId,

            };

            backgroundJobService.Enqueue<IUserAnalyticsService>(u => u.IncrementClickCountAsync(domainEvent.ShortUrl, cancellationToken));

            backgroundJobService.Enqueue<IUrlAccessLogService>(l => l.LogAccessAsync(accessDto, cancellationToken));

            return Task.CompletedTask;
        }
    }
}
