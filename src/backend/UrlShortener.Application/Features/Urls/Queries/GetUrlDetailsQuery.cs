using MediatR;
using UrlShortener.Application.Features.Urls.Dtos;

namespace UrlShortener.Application.Features.Urls.Queries
{
    public class GetUrlDetailsQuery : IRequest<UrlDetailsDto>
    {

        public string ShortUrl { get; }

        public GetUrlDetailsQuery(string shortUrl)
        {
            ShortUrl = shortUrl;
        }
    }
}
