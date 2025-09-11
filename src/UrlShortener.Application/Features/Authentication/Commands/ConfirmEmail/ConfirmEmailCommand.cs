using MediatR;

namespace Project.Application.Features.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<string>
    {

        public string UserId { get; set; }

        public string Token { get; set; }

        public ConfirmEmailCommand(string UserId, string Token)
        {
            this.UserId = UserId;
            this.Token = Token;
        }
    }
}
