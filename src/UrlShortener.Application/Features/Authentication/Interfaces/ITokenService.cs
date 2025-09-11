using Project.Application.Features.Authentication.Dtos;

namespace Project.Application.Features.Authentication.Interfaces
{
    public interface ITokenService
    {

        string GenerateAccessToken(string UserName, string email, string id, IList<string> roles);


        Task<string> CreateRefreshTokenForDevice(string userId, string DeviceId);

        Task<LogInResponseWithRefreshToken> RefreshAccessTokenAsync(string refreshToken, string DeviceId);


        Task RevokeRefreshTokenByToken(string token, string DeviceId);
    }
}
