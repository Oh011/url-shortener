using MediatR;

namespace UrlShortener.Application.Features.Urls.Commands
{
    public class DeleteUrlCommand : IRequest
    {
        public string ShortUrl { get; set; } = default!;


        public DeleteUrlCommand(string shortUrl) { this.ShortUrl = shortUrl; }
    }

}
