using MediatR;
using Project.Application.Features.Authentication.Interfaces;

namespace Project.Application.Features.Authentication.Commands.ResendConfirmationEmail
{
    internal class ResendConfirmationEmailCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ResendConfirmationEmailCommand, string>
    {
        public async Task<string> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            return await authenticationService.ResendConfirmationEmail(request);
        }
    }
}
