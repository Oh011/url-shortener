using MediatR;
using Project.Application.Features.Urls.Dtos;

namespace Project.Application.Features.Urls.Commands.UpdateUrl
{
    public class UpdateUrlCommand : IRequest<UrlDetailsDto>
    {

        public string ShortUrl { get; set; } = default!;
        public DateTime NewExpiresAt { get; set; }
    }
}
