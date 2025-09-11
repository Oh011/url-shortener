using MediatR;
using Project.Application.Common.Events;
using Project.Application.Common.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Application.Features.Analytics.Dtos;
using Project.Domain.DomainEvents;

namespace Project.Application.Features.Urls.EventHandlers
{
    internal class UrlUpdatedEventHandler : INotificationHandler<DomainEventNotifications<UrlUpdatedEvent>>
    {


        private readonly IBackgroundJobService backgroundJobService;


        public UrlUpdatedEventHandler(IBackgroundJobService backgroundJobService)
        {

            this.backgroundJobService = backgroundJobService;
        }
        public Task Handle(DomainEventNotifications<UrlUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;


            var dto = new UserAnalyticsDto
            {

                ShortUrl = domainEvent.ShortUrl,
                UserId = domainEvent.UserId,
                ExpiresAt = domainEvent.Expiration,
                OriginalUrl = domainEvent.OriginalUrl,
            };




            backgroundJobService.Enqueue<IUserAnalyticsService>(s => s.UpdateAnalyticsRecordAsync(dto, cancellationToken));

            return Task.CompletedTask;
        }
    }


}
