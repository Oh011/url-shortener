namespace UrlShortener.Application.Abstrcations.Persistence
{
    public interface IUniqueIdGenerator
    {


        public Task<long> GetNextId();
    }
}
