using MediatR;

namespace Project.Application.Features.Authentication.Commands.Register
{
    public class RegisterUserCommand : IRequest<string>
    {

        public string Email { get; set; }

        public string Password { get; set; }


        public string Username { get; set; }



    }
}
