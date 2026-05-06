using MediatR;
using UrlShortener.Application.Features.UsersAnalytics.Dtos;

namespace UrlShortener.Application.Features.UsersAnalytics.queries
{


    public class GetTopUserUrlsQuery : IRequest<IEnumerable<TopUrlDto>>
    {
        public int Count { get; set; } = 5;

        public GetTopUserUrlsQuery(int count) { this.Count = count; }
    }

}
