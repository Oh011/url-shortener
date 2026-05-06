using MediatR;
using UrlShortener.Application.Features.Urls.Dtos;

namespace UrlShortener.Application.Features.Urls.Commands
{
    public class CreateShortUrlCommand : IRequest<ShortUrlDto>
    {

        public string LongUrl { get; set; }
        public string? CustomAlias { get; set; }
        public DateTime? ExpirationDate { get; set; } // optional
    }
}
