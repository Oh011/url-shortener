using MediatR;
using Project.Application.Features.Analytics.Dtos;

namespace Project.Application.Features.Analytics.queries.UserAnalyticsSummary
{
    public class GetUserAnalyticsSummaryQuery : IRequest<UserAnalyticsSummaryDto>
    {
    }

}
