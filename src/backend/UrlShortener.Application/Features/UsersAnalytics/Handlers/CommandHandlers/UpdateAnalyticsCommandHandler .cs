using MediatR;
using UrlShortener.Application.Common.Factories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Application.Features.UsersAnalytics.Commands;

namespace UrlShortener.Application.Features.UsersAnalytics.Handlers.CommandHandlers
{

    internal class UpdateAnalyticsCommandHandler
    : IRequestHandler<UpdateAnalyticsCommand>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UpdateAnalyticsCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task Handle(UpdateAnalyticsCommand request, CancellationToken cancellationToken)
        {
            var uow = _unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repo = uow.UserAnalyticsRepo;

            var record = await repo.FirstOrDefaultAsync(
                a => a.UserId == request.UserId && a.ShortUrl == request.ShortUrl
            );

            if (record == null) return;

            record.ShortUrl = request.ShortUrl;
            record.UserId = request.UserId;
            record.OriginalUrl = request.OriginalUrl;
            record.ExpiresAt = request.ExpiresAt;

            await uow.CommitAsync(cancellationToken);
        }
    }

}
