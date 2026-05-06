using MediatR;
using UrlShortener.Application.Features.UsersAnalytics.Dtos;

namespace UrlShortener.Application.Features.UsersAnalytics.queries
{
    public class GetUserAnalyticsSummaryQuery : IRequest<UserAnalyticsSummaryDto>
    {
    }

}
