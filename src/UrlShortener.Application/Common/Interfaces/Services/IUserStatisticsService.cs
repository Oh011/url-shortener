namespace Project.Application.Common.Interfaces.Services
{
    public interface IUserStatisticsService
    {
        Task UpdateTotalUrlsAsync(string userId, int change, CancellationToken cancellationToken = default);
        Task IncrementTotalClicksAsync(string userId, long count = 1, CancellationToken cancellationToken = default);
    }

}
