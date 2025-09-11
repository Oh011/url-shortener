using MediatR;
using Project.Application.Features.Urls.Dtos;

namespace Project.Application.Features.Urls.Commands.CreateShortUrl
{
    public class CreateShortUrlCommand : IRequest<ShortUrlDto>
    {

        public string LongUrl { get; set; }
        public string? CustomAlias { get; set; }
        public DateTime? ExpirationDate { get; set; } // optional
    }
}
