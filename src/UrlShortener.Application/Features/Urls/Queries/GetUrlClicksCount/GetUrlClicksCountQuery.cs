using MediatR;
using Project.Application.Features.Urls.Dtos;

namespace Project.Application.Features.Urls.Queries.GetUrlClicksCount
{
    public class GetUrlClicksCountQuery : IRequest<UrlClickCountsDto>
    {

        public string ShortUrl { get; set; }


        public GetUrlClicksCountQuery(string shortUrl) { this.ShortUrl = shortUrl; }
    }
}
