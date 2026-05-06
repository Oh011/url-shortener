using UrlShortener.Application.Common.Factories.Interfaces;
using UrlShortener.Infrastructure.Persistence.Context;
using UrlShortener.Infrastructure.Persistence.Repositories;
using System.Collections.Concurrent;
using UrlShortener.Infrastructure.Persistence.Interfaces;
using UrlShortener.Application.Abstrcations.Persistence;



namespace UrlShortener.Infrastructure.Persistence.Factories
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory, IDisposable
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IShardManager _shardManager;


        private AppUnitOfWork? _appDbUnitOfWork;
        private readonly ConcurrentDictionary<string, ShardUnitOfWork> _shardUnitOfWorks = new();

        public UnitOfWorkFactory(ApplicationDbContext appDbContext, IShardManager shardManager)
        {
            _appDbContext = appDbContext;
            _shardManager = shardManager;
        }

        public IAppUnitOfWork CreateAppDbUnitOfWork()
        {

            if (_appDbUnitOfWork == null)
                _appDbUnitOfWork = new AppUnitOfWork(_appDbContext);

            return _appDbUnitOfWork;
        }

        public IEnumerable<IShardUnitOfWork> CreateAllShardUnitOfWorks()
        {
            var list = new List<IShardUnitOfWork>();
            foreach (var node in _shardManager.GetAllNodes())
            {
                var uow = _shardUnitOfWorks.GetOrAdd(node.Name, k => new ShardUnitOfWork(node));
                list.Add(uow);
            }
            return list;
        }


        public IShardUnitOfWork CreateShardUnitOfWork(string key)
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
