using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Application.Features.UrlAccessLogs.Commands
{
   

    public record LogUrlAccessCommand(
        long UrlId,
        string ShortUrl,
        string IpAddress,
        string Referrer,
        string UserAgent
    ) : IRequest;

}
