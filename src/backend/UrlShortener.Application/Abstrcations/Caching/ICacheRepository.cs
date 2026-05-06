namespace UrlShortener.Application.Abstrcations.Caching
{
    public interface ICacheRepository
    {



        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        public Task<T?> GetAsync<T>(string key);


        public Task RemoveAsync(string key);


    }
}
