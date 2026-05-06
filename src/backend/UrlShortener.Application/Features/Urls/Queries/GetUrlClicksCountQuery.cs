using MediatR;
using UrlShortener.Application.Features.Urls.Dtos;

namespace UrlShortener.Application.Features.Urls.Queries
{
    public class GetUrlClicksCountQuery : IRequest<UrlClickCountsDto>
    {

        public string ShortUrl { get; set; }


        public GetUrlClicksCountQuery(string shortUrl) { this.ShortUrl = shortUrl; }
    }
}
