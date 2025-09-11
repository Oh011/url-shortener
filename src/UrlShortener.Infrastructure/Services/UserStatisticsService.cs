using Project.Application.Common.Factories.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Domain.Entities;

namespace Project.Infrastructure.Services
{
    internal class UserStatisticsService : IUserStatisticsService
    {

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public UserStatisticsService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }
        public async Task IncrementTotalClicksAsync(string userId, long count = 1, CancellationToken cancellationToken = default)
        {


            var unitOfWork = unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repository = unitOfWork.GetRepository<UserStatistics, string>();

            var userStatisticsRecord = await repository.GetById(userId);


            if (userStatisticsRecord != null)
            {
                userStatisticsRecord.TotalClicks += count;
                userStatisticsRecord.LastActiveAt = DateTime.UtcNow;
            }

            await unitOfWork.Commit();
        }

        public async Task UpdateTotalUrlsAsync(string userId, int change, CancellationToken cancellationToken = default)
        {

            var unitOfWork = unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repository = unitOfWork.GetRepository<UserStatistics, string>();

            var userStatisticsRecord = await repository.GetById(userId);


            if (userStatisticsRecord != null)
            {
                userStatisticsRecord.TotalUrls += change;
                if (userStatisticsRecord.TotalUrls < 0)
                    userStatisticsRecord.TotalUrls = 0; // optional, prevent negative

            }

            else if (change > 0)
            {
                {

                    var record = new UserStatistics
                    {
                        UserId = userId,
                        TotalUrls = change

                    };

                    await repository.AddAsync(record);
                }


            }
            await unitOfWork.Commit();
        }
    }
}
