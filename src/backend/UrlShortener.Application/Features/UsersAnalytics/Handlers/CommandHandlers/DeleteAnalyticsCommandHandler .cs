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


    internal class DeleteAnalyticsCommandHandler
    : IRequestHandler<DeleteAnalyticsCommand>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public DeleteAnalyticsCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task Handle(DeleteAnalyticsCommand request, CancellationToken cancellationToken)
        {
            var uow = _unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repo = uow.UserAnalyticsRepo;

            var record = await repo.FirstOrDefaultAsync(
                a => a.UserId == request.UserId && a.ShortUrl == request.ShortUrl
            );

            if (record == null) return;

            repo.Delete(record);
            await uow.CommitAsync(cancellationToken);
        }
    }

}
