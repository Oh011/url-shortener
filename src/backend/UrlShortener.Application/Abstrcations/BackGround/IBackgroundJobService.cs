using System.Linq.Expressions;

namespace UrlShortener.Application.Abstrcations.BackGround
{
    public interface IBackgroundJobService
    {


        public void Enqueue<TService>(Expression<Func<TService, Task>> methodCall
            ) where TService : class;
    }
}
