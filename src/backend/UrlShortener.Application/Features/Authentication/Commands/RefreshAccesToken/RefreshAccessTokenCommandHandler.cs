using MediatR;
using UrlShortener.Application.Features.Authentication.Dtos;
using UrlShortener.Application.Features.Authentication.Interfaces;

namespace UrlShortener.Application.Features.Authentication.Commands.RefreshAccesToken
{
    internal class RefreshAccessTokenCommandHandler : IRequestHandler<RefreshAccessTokenCommand, LogInResponseWithRefreshToken>
    {


        private readonly ITokenService tokenService;

        public async Task<LogInResponseWithRefreshToken> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {


            return await tokenService.RefreshAccessTokenAsync(request.RefreshToken, request.DeviceId);
        }
    }
}
