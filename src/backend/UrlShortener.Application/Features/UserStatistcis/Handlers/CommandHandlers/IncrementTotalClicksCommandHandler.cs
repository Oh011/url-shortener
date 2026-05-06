using MediatR;
using UrlShortener.Application.Common.Factories.Interfaces;
using UrlShortener.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Application.Features.UserStatistcis.commands;

namespace UrlShortener.Application.Features.UserStatistcis.Handlers.CommandHandlers
{
    internal class IncrementTotalClicksCommandHandler
      : IRequestHandler<IncrementTotalClicksCommand>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public IncrementTotalClicksCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task Handle(IncrementTotalClicksCommand request, CancellationToken cancellationToken)
        {
            var uow = _unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repo = uow.UserStatisticsRepo;

            var userStats = await repo.GetById(request.UserId);

            if (userStats != null)
            {
                userStats.TotalClicks += request.Count;
                userStats.LastActiveAt = DateTime.UtcNow;
            }

            await uow.CommitAsync(cancellationToken);
        }
    }

}
