using MediatR;

namespace Project.Application.Features.Authentication.Commands.ResendConfirmationEmail
{
    public class ResendConfirmationEmailCommand : IRequest<string>
    {

        public string Email { get; set; }


        public ResendConfirmationEmailCommand(string Email)
        {
            this.Email = Email;
        }
    }
}
