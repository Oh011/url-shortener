using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Application.Features.UsersAnalytics.Commands
{
    public record UpdateAnalyticsCommand(
    string UserId,
    string ShortUrl,
    string OriginalUrl,
    DateTime? ExpiresAt
) : IRequest;

}
