using MediatR;
using Project.Application.Features.Authentication.Interfaces;

namespace Project.Application.Features.Authentication.Commands.LogOut
{
    internal class LogOutCommandHandler : IRequestHandler<LogOutCommand, string>
    {

        private readonly ITokenService _tokenservice;


        public LogOutCommandHandler(ITokenService tokenService)
        {

            _tokenservice = tokenService;
        }
        public async Task<string> Handle(LogOutCommand request, CancellationToken cancellationToken)
        {


            await _tokenservice.RevokeRefreshTokenByToken(request.RefreshToken, request.DeviceId);

            return "Logged out successfully.";
        }
    }
}
