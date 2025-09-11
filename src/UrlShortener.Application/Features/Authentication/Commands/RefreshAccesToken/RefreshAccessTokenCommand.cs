using MediatR;
using Project.Application.Features.Authentication.Dtos;

namespace Project.Application.Features.Authentication.Commands.RefreshAccesToken
{
    public class RefreshAccessTokenCommand : IRequest<LogInResponseWithRefreshToken>
    {
        public string? RefreshToken { get; set; }
        public string DeviceId { get; set; } = default!;
    }
}
