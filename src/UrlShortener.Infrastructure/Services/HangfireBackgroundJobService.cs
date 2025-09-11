using Hangfire;
using Project.Application.Common.Interfaces;
using System.Linq.Expressions;

namespace Project.Infrastructure.Services
{
    internal class HangFireBackgroundJobService : IBackgroundJobService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public HangFireBackgroundJobService(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public void Enqueue<TService>(Expression<Func<TService, Task>> methodCall) where TService : class
        {
            _backgroundJobClient.Enqueue<TService>(methodCall);
        }
    }
}
