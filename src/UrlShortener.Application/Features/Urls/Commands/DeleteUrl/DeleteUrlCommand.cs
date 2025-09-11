using MediatR;

namespace Project.Application.Features.Urls.Commands.DeleteUrl
{
    public class DeleteUrlCommand : IRequest
    {
        public string ShortUrl { get; set; } = default!;


        public DeleteUrlCommand(string shortUrl) { this.ShortUrl = shortUrl; }
    }

}
