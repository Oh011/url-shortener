using MediatR;
using UrlShortener.Application.Features.Authentication.Dtos;

namespace UrlShortener.Application.Features.Authentication.Commands.LogIn
{
    public class LogInUserCommand : IRequest<LogInResponseWithRefreshToken>
    {


        public string Email { get; set; }


        public string Password { get; set; }


        public string DeviceId { get; set; }
    }
}
