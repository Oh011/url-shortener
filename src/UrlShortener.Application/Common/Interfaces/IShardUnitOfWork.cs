namespace Project.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {


        public Task<int> Commit(CancellationToken cancellationToken = default);


        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class;
    }
}
