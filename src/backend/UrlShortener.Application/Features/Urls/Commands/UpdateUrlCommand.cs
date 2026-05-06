using MediatR;
using UrlShortener.Application.Features.Urls.Dtos;

namespace UrlShortener.Application.Features.Urls.Commands
{
    public class UpdateUrlCommand : IRequest<UrlDetailsDto>
    {

        public string ShortUrl { get; set; } = default!;
        public DateTime NewExpiresAt { get; set; }
    }
}
