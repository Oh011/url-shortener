using MediatR;
using Project.Application.Features.Authentication.Dtos;
using Project.Application.Features.Authentication.Interfaces;

namespace Project.Application.Features.Authentication.Commands.RefreshAccesToken
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
