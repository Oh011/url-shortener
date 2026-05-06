using MediatR;
using UrlShortener.Application.Features.Authentication.Dtos;

namespace UrlShortener.Application.Features.Authentication.Commands.RefreshAccesToken
{
    public class RefreshAccessTokenCommand : IRequest<LogInResponseWithRefreshToken>
    {
        public string? RefreshToken { get; set; }
        public string DeviceId { get; set; } = default!;
    }
}
