using MediatR;
using Project.Application.Features.Authentication.Interfaces;

namespace Project.Application.Features.Authentication.Commands.ConfirmEmail
{
    internal class ConfirmEmailCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ConfirmEmailCommand, string>
    {
        public async Task<string> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {



            return await authenticationService.ConformEmail(request);
        }
    }
}
