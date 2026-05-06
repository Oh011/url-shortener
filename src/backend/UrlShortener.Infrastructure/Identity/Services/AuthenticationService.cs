using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using UrlShortener.Application.Exceptions;
using UrlShortener.Application.Features.Authentication.Commands.ConfirmEmail;
using UrlShortener.Application.Features.Authentication.Commands.LogIn;
using UrlShortener.Application.Features.Authentication.Commands.Register;
using UrlShortener.Application.Features.Authentication.Commands.ResendConfirmationEmail;
using UrlShortener.Application.Features.Authentication.Dtos;
using UrlShortener.Application.Features.Authentication.Interfaces;
using UrlShortener.Infrastructure.Identity.Entities;
using Shared.Dtos;
using System.Text;


using IAuthenticationService = UrlShortener.Application.Features.Authentication.Interfaces.IAuthenticationService;
using UrlShortener.Application.Abstrcations.Messaging;

namespace UrlShortener.Infrastructure.Identity.Services
{
    internal class AuthenticationService(UserManager<ApplicationUser> userManager, ITokenService tokenService, IEmailService emailService, IConfiguration configuration) : IAuthenticationService
    {
        public async Task<string> ConformEmail(ConfirmEmailCommand command)
        {


            var user = await userManager.FindByIdAsync(command.UserId);
            if (user == null) throw new NotFoundException("User Not Found");

            var decodedBytes = WebEncoders.Base64UrlDecode(command.Token.Trim());
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);


            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                throw new BadRequestException("Email confirmation failed.");

            return "Email confirmed successfully! You can now log in.";
        }

        public async Task<string> Register(RegisterUserCommand command)
        {

            var userExists = await userManager.FindByEmailAsync(command.Email);


            if (await userManager.FindByEmailAsync(command.Email) != null)
            {

                throw new ConflictException("Email already exists.");
            }

            var user = new ApplicationUser
            {
                Email = command.Email,
                UserName = command.Username,

            };


            var results = await userManager.CreateAsync(user, command.Password);


            if (!results.Succeeded)
            {


                var errors = results.Errors.GroupBy(e => e.Code).
                    ToDictionary(g => MapToField(g.Key), g => g.Select(e => e.Description).ToList());


                throw new ValidationException(errors, "Registration Failed");

            }


            await userManager.AddToRoleAsync(user, "User");



            //await SendConfirmationEmail(user);


            return "Account created successfully. A confirmation email has been sent.";
        }

        public async Task<string> ResendConfirmationEmail(ResendConfirmationEmailCommand command)
        {


            var user = await userManager.FindByEmailAsync(command.Email);

            if (user == null) throw new NotFoundException("User Not Found");

            if (await userManager.IsEmailConfirmedAsync(user))
                throw new BadRequestException("Email is already confirmed.");



            await SendConfirmationEmail(user);

            return "A confirmation email has been sent.";

        }


        private async Task SendConfirmationEmail(ApplicationUser user)
        {

            var baseUrl = configuration.GetSection("BaseUrl").Value;


            var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));


            var confirmationLink = $"{baseUrl}/api/auth/confirm-email?userId={user.Id}&token={encodedToken}";

            await emailService.SendEmailAsync(new EmailMessage
            {

                Body = confirmationLink,
                To = user.Email,
                Subject = "Email Confirmation",
            });



        }

        private string MapToField(string code)
        {
            code = code.ToLower();

            if (code.Contains("email"))
                return "Email";
            if (code.Contains("password"))
                return "Password";
            if (code.Contains("username"))
                return "Username";

            return "Identity";
        }

        public async Task<LogInResponseWithRefreshToken> LogIn(LogInUserCommand command)
        {

            var user = await userManager.FindByEmailAsync(command.Email);


            if (user == null) throw new UnAuthorizedException("Invalid credentials");



            //if (await userManager.IsEmailConfirmedAsync(user)) throw new ForbiddenException("Email not confirmed");


            if (await userManager.IsLockedOutAsync(user)) throw new AccountLockedException();
           
            





            var result = await userManager.CheckPasswordAsync(user, command.Password);



            if (!result)
            {

                await userManager.AccessFailedAsync(user);

                if (await userManager.IsLockedOutAsync(user)) throw new AccountLockedException();


                throw new UnAuthorizedException("Invalid credentials.");
            }


            await userManager.ResetAccessFailedCountAsync(user);


            var roles = await userManager.GetRolesAsync(user);

            var accessToken = tokenService.GenerateAccessToken(user.UserName, user.Email, user.Id, roles);

            var refreshToken = await tokenService.CreateRefreshTokenForDevice(user.Id, command.DeviceId);




            return new LogInResponseWithRefreshToken
            {
                Response = new LogInResponseDto
                {
                    AccessToken = accessToken,
                    Email = user.Email,
                    UserName = user.UserName,
                    UserId = user.Id,

                },
                RefreshToken = refreshToken
            };
        }
    }
}


