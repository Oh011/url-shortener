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
    internal class UpdateTotalUrlsCommandHandler
    : IRequestHandler<UpdateTotalUrlsCommand>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public UpdateTotalUrlsCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task Handle(UpdateTotalUrlsCommand request, CancellationToken cancellationToken)
        {
            var uow = _unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repo = uow.UserStatisticsRepo;

            var userStats = await repo.GetById(request.UserId);

            if (userStats != null)
            {
                userStats.TotalUrls += request.Change;

                if (userStats.TotalUrls < 0)
                    userStats.TotalUrls = 0;
            }
            else if (request.Change > 0)
            {
                var newRecord = new UserStatistics
                {
                    UserId = request.UserId,
                    TotalUrls = request.Change,
                    TotalClicks = 0
                };

                await repo.AddAsync(newRecord);
            }

            await uow.CommitAsync(cancellationToken);
        }
    }

}
