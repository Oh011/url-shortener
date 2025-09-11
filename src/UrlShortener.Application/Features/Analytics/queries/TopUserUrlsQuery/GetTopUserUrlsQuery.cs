using MediatR;
using Project.Application.Features.Analytics.Dtos;

namespace Project.Application.Features.Analytics.queries.TopUserUrlsQuery
{


    public class GetTopUserUrlsQuery : IRequest<IEnumerable<TopUrlDto>>
    {
        public int Count { get; set; } = 5;

        public GetTopUserUrlsQuery(int count) { this.Count = count; }
    }

}
