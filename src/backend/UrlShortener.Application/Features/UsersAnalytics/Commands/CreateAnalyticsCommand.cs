using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Application.Features.UsersAnalytics.Commands
{
    public record CreateAnalyticsCommand
    (

       string UserId,
           string OriginalUrl,
           string ShortUrl,
             DateTime ExpiresAt
    ):IRequest;
}
