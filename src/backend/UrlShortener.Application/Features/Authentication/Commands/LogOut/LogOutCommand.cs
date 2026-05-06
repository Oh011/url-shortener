using MediatR;

namespace UrlShortener.Application.Features.Authentication.Commands.LogOut
{
    public class LogOutCommand : IRequest<string>
    {
        public string? RefreshToken { get; set; }
        public string DeviceId { get; set; } = default!;
    }
}
