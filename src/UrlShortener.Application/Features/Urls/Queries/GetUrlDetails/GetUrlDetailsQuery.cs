using MediatR;
using Project.Application.Features.Urls.Dtos;

namespace Project.Application.Features.Urls.Queries.GetUrlDetails
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
