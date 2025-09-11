using Project.Application.Common.Factories.Interfaces;
using Project.Application.Common.Interfaces;
using Project.Infrastructure.Persistence.Context;
using Project.Infrastructure.Persistence.Repositories;
using System.Collections.Concurrent;


namespace Project.Infrastructure.Services.Factories
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory, IDisposable
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IShardManager _shardManager;


        private UnitOfWork? _appDbUnitOfWork;
        private readonly ConcurrentDictionary<string, ShardUnitOfWork> _shardUnitOfWorks = new();

        public UnitOfWorkFactory(ApplicationDbContext appDbContext, IShardManager shardManager)
        {
            _appDbContext = appDbContext;
            _shardManager = shardManager;
        }

        public IUnitOfWork CreateAppDbUnitOfWork()
        {

            if (_appDbUnitOfWork == null)
                _appDbUnitOfWork = new UnitOfWork(_appDbContext);

            return _appDbUnitOfWork;
        }

        public IEnumerable<IUnitOfWork> CreateAllShardUnitOfWorks()
        {
            var list = new List<IUnitOfWork>();
            foreach (var node in _shardManager.GetAllNodes())
            {
                var uow = _shardUnitOfWorks.GetOrAdd(node.Name, k => new ShardUnitOfWork(node));
                list.Add(uow);
            }
            return list;
        }


        public IUnitOfWork CreateShardUnitOfWork(string key)
        {

            var x = _shardManager.GetNode(key);
            return _shardUnitOfWorks.GetOrAdd(x.Name, k => new ShardUnitOfWork(x));
        }


        public void Dispose()
        {

            foreach (var shardUow in _shardUnitOfWorks.Values)
            {
                shardUow.Dispose();
            }

            _shardUnitOfWorks.Clear();

        }
    }
}
