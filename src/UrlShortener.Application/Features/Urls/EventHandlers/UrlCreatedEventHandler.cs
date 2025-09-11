using MediatR;
using Project.Application.Common.Events;
using Project.Application.Common.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Application.Features.Analytics.Dtos;
using Project.Domain.DomainEvents;

namespace Project.Application.Features.Urls.EventHandlers
{
    internal class UrlCreatedEventHandler : INotificationHandler<DomainEventNotifications<UrlCreatedEvent>>
    {


        private readonly IBackgroundJobService backgroundJobService;


        public UrlCreatedEventHandler(IBackgroundJobService backgroundJobService)
        {

            this.backgroundJobService = backgroundJobService;
        }
        public Task Handle(DomainEventNotifications<UrlCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;


            if (domainEvent.UserId != null)
            {

                backgroundJobService.Enqueue<IUserStatisticsService>(us => us.UpdateTotalUrlsAsync(notification.DomainEvent.UserId, 1, cancellationToken));

                var userAnalyticsDto = new UserAnalyticsDto
                {
                    UserId = domainEvent.UserId,
                    OriginalUrl = domainEvent.OriginalUrl,
                    ShortUrl = domainEvent.ShortUrl,
                    ExpiresAt = domainEvent.ExpiresAt

                };

                backgroundJobService.Enqueue<IUserAnalyticsService>(us => us.CreateAnalyticsRecordAsync(userAnalyticsDto, cancellationToken));

            }




            return Task.CompletedTask;
        }
    }
}
