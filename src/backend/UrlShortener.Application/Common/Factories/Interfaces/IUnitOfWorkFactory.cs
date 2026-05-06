using UrlShortener.Application.Abstrcations.Persistence;

namespace UrlShortener.Application.Common.Factories.Interfaces
{
    public interface IUnitOfWorkFactory
    {


        IEnumerable<IShardUnitOfWork> CreateAllShardUnitOfWorks();
        IAppUnitOfWork CreateAppDbUnitOfWork();
        IShardUnitOfWork CreateShardUnitOfWork(string key);
    }
}
