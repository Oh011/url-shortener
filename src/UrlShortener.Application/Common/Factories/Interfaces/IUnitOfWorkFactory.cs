using Project.Application.Common.Interfaces;

namespace Project.Application.Common.Factories.Interfaces
{
    public interface IUnitOfWorkFactory
    {


        IEnumerable<IUnitOfWork> CreateAllShardUnitOfWorks();
        IUnitOfWork CreateAppDbUnitOfWork();
        IUnitOfWork CreateShardUnitOfWork(string key);
    }
}
