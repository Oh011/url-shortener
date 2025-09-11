using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project.Application.Common.Factories.Interfaces;
using Project.Application.Exceptions;
using Project.Application.Features.Authentication.Dtos;
using Project.Application.Features.Authentication.Interfaces;
using Project.Infrastructure.Identity.Entities;
using Shared.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Project.Infrastructure.Identity.Services
{
    public class TokenService(UserManager<ApplicationUser> userManager, IOptions<JwtOptions> options, IUnitOfWorkFactory unitOfWorkFactory) : ITokenService
    {
        public async Task<string> CreateRefreshTokenForDevice(string userId, string DeviceId)
        {
            var unitOfWork = unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repository = unitOfWork.GetRepository<RefreshToken, int>();


            var oldToken = (await repository.
             FindAsync(t => t.UserId == userId && t.IsRevoked == false && t.DeviceId == DeviceId, true)

             ).FirstOrDefault();


            if (oldToken != null)
            {

                await RevokeRefreshTokenAsync(oldToken);
            }


            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(14), // Typically 7 days expiration
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false,
                DeviceId = DeviceId,

            };

            await repository.AddAsync(refreshToken);

            await unitOfWork.Commit();


            return refreshToken.Token;
        }

        public string GenerateAccessToken(string UserName, string email, string id, IList<string> roles)
        {


            var jwtOptions = options.Value;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,UserName),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, id),

            };




            foreach (var role in roles)
            {

                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            //signature = Base64Url( HMACSHA256(header.payload, secretKey) ).



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));


            var SignCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var tokenDescriptor = new SecurityTokenDescriptor()
            {

                Subject = new ClaimsIdentity(claims),
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audiance,
                Expires = DateTime.UtcNow.AddHours(jwtOptions.ExpirationInHours),

                SigningCredentials = SignCred



            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var Token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(Token);
        }

        //whiotPg4R5RMRRVK8o c1/o5qA3iwRV5510XEmQE4n7DDvQvhVMHVHzrJ9xxRpHzqcyuFUVIqzhXdv2In7XiVw=="
        //whiotPg4R5RMRRVK8o+c1/o5qA3iwRV5510XEmQE4n7DDvQvhVMHVHzrJ9xxRpHzqcyuFUVIqzhXdv2In7XiVw==
        //whiotPg4R5RMRRVK8o%2Bc1%2Fo5qA3iwRV5510XEmQE4n7DDvQvhVMHVHzrJ9xxRpHzqcyuFUVIqzhXdv2In7XiVw%3D%3D

        private async Task<RefreshToken> ValidateRefreshTokenOrThrow(string token, string deviceId)
        {

            var unitOfWork = unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repository = unitOfWork.GetRepository<RefreshToken, int>();

            var refreshToken = (await repository.FindAsync(t => t.Token == token && t.DeviceId == deviceId))
              .FirstOrDefault();



            if (refreshToken == null || refreshToken.IsExpired)
                throw new UnAuthorizedException("Invalid or expired refresh token.");



            if (refreshToken.IsRevoked)
                throw new ForbiddenException("This refresh token has already been revoked.");

            return refreshToken;
        }


        private async Task RevokeRefreshTokenAsync(RefreshToken token)
        {

            var unitOfWork = unitOfWorkFactory.CreateAppDbUnitOfWork();
            var repository = unitOfWork.GetRepository<RefreshToken, int>();
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;

            repository.Update(token);
            await unitOfWork.Commit();


        }


        public async Task<LogInResponseWithRefreshToken> RefreshAccessTokenAsync(string refreshToken, string DeviceId)
        {
            var storedToken = await ValidateRefreshTokenOrThrow(refreshToken, DeviceId);

            var user = await userManager.FindByIdAsync(storedToken.UserId);

            if (user == null)
                throw new NotFoundException("User not found.");

            var roles = await userManager.GetRolesAsync(user);

            var accessToken = GenerateAccessToken(user.UserName, user.Email, user.Id, roles);


            await this.RevokeRefreshTokenAsync(storedToken);


            var newRefreshToken = await CreateRefreshTokenForDevice(user.Id, DeviceId);



            return new LogInResponseWithRefreshToken
            {
                Response = new LogInResponseDto
                {
                    AccessToken = accessToken,
                    Email = user.Email,
                    UserName = user.UserName,
                    UserId = user.Id,



                },
                RefreshToken = newRefreshToken
            };
        }

        public async Task RevokeRefreshTokenByToken(string token, string DeviceId)
        {


            var refreshToken = await ValidateRefreshTokenOrThrow(token, DeviceId);

            await RevokeRefreshTokenAsync(refreshToken);
        }


        private string GenerateRefreshToken(int size = 64)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(size);

            // Convert to Base64 string
            return Convert.ToBase64String(randomBytes);
        }
    }
}
