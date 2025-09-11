using MediatR;
using Project.Application.Common.Events;
using Project.Application.Common.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Domain.DomainEvents;

namespace Project.Application.Features.Urls.EventHandlers
{
    internal class UrlDeletedEventHandler : INotificationHandler<DomainEventNotifications<UrlDeletedEvent>>
    {


        private readonly IBackgroundJobService backgroundJobService;


        public UrlDeletedEventHandler(IBackgroundJobService backgroundJobService)
        {

            this.backgroundJobService = backgroundJobService;
        }
        public Task Handle(DomainEventNotifications<UrlDeletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;


            backgroundJobService.Enqueue<IUserStatisticsService>(s => s.UpdateTotalUrlsAsync(domainEvent.UserId, -1, cancellationToken));


            backgroundJobService.Enqueue<IUserAnalyticsService>(s => s.DeleteAnalyticsRecordAsync(domainEvent.UserId, domainEvent.ShortUrl, cancellationToken));

            return Task.CompletedTask;
        }
    }
}
