using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Application.Features.UsersAnalytics.Commands
{
    public record DeleteAnalyticsCommand(string UserId, string ShortUrl) : IRequest;

}
