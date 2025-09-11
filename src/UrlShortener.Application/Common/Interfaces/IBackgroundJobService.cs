using System.Linq.Expressions;

namespace Project.Application.Common.Interfaces
{
    public interface IBackgroundJobService
    {


        public void Enqueue<TService>(Expression<Func<TService, Task>> methodCall
            ) where TService : class;
    }
}
