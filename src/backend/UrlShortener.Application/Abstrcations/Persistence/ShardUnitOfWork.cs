using UrlShortener.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Application.Abstrcations.Persistence
{

    public interface IShardUnitOfWork
    {
        IGenericRepository<Url, long> UrlRepo { get; }
        IGenericRepository<UrlAccessLog, string> UrlAccessLogRepo { get; }
        Task<int> CommitAsync(CancellationToken ct = default);
    }

}
