using MediatR;
using Project.Application.Features.Authentication.Dtos;

namespace Project.Application.Features.Authentication.Commands.LogIn
{
    public class LogInUserCommand : IRequest<LogInResponseWithRefreshToken>
    {


        public string Email { get; set; }


        public string Password { get; set; }


        public string DeviceId { get; set; }
    }
}
