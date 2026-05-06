using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortener.Application.Features.UserStatistcis.commands
{
    public record IncrementTotalClicksCommand(
     string UserId,
     long Count = 1
 ) : IRequest;

}
