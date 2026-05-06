using MediatR;
using UrlShortener.Application.Common.Factories.Interfaces;
using UrlShortener.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Application.Features.UsersAnalytics.Commands;


namespace UrlShortener.Application.Features.UsersAnalytics.Handlers.CommandHandlers
{
    internal class CreateAnalyticsCommandHandler : IRequestHandler<CreateAnalyticsCommand>
    {


        private readonly IUnitOfWorkFactory _unitOfWorkFactory;



        public CreateAnalyticsCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {

            _unitOfWorkFactory = unitOfWorkFactory;

        }

        

        public async Task Handle(CreateAnalyticsCommand request, CancellationToken cancellationToken)
        {


            var appUnitOfWork = _unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repo = appUnitOfWork.UserAnalyticsRepo;

            var existing = await repo.ExistsAsync(
                a => a.UserId == request.UserId && a.ShortUrl == request.ShortUrl
            );

            if (!existing)
            {
                var record = new UserAnalytics
                {
                    UserId = request.UserId,
                    ShortUrl = request.ShortUrl,
                    OriginalUrl = request.OriginalUrl,
                    ClickCount = 0,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = request.ExpiresAt,
                    LastAccessedAt = null,

                };

                await repo.AddAsync(record);
                await appUnitOfWork.CommitAsync(cancellationToken);
            }
        }
    }
}
