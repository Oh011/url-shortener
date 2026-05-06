using MediatR;
using UrlShortener.Application.Features.Authentication.Interfaces;

namespace UrlShortener.Application.Features.Authentication.Commands.ResendConfirmationEmail
{
    internal class ResendConfirmationEmailCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ResendConfirmationEmailCommand, string>
    {
        public async Task<string> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            return await authenticationService.ResendConfirmationEmail(request);
        }
    }
}
