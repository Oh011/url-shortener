using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Exceptions;
using UrlShortener.Application.Features.Authentication.Commands.ConfirmEmail;
using UrlShortener.Application.Features.Authentication.Commands.LogIn;
using UrlShortener.Application.Features.Authentication.Commands.LogOut;
using UrlShortener.Application.Features.Authentication.Commands.RefreshAccesToken;
using UrlShortener.Application.Features.Authentication.Commands.Register;
using UrlShortener.Application.Features.Authentication.Commands.ResendConfirmationEmail;
using UrlShortener.Application.Features.Authentication.Dtos;
using Shared;
using Url_Shortener.Dtos.Authentication;

namespace Url_Shortener.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController(IMediator mediator) : ControllerBase
    {



        [HttpPost("register")]

        public async Task<SuccessMessage> Register([FromBody] RegisterUserCommand command)
        {

            var result = await mediator.Send(command);
            return ApiResponseFactory.Success(result);
        }


        [HttpGet("confirm-email")]
        public async Task<SuccessMessage> ConfirmEmail(string userId, string token)
        {

            var result = await mediator.Send(new ConfirmEmailCommand(userId, token));
            return ApiResponseFactory.Success(result);
        }



        [HttpPost("resend-confirmation")]
        public async Task<SuccessMessage> ResendConfirmationEmail([FromBody] ResendConfirmationEmailCommand command)
        {

            var result = await mediator.Send(command);


            return ApiResponseFactory.Success(result);

        }


        /// <summary>
        /// Logs in a user and returns a JWT and refresh token.
        /// </summary>
        /// <param name="request">User login request DTO</param>
        /// <returns>Access token and refresh token</returns>

        [HttpPost("login")]


        public async Task<ActionResult<SuccessWithData<LogInResponseDto>>> LogIn([FromBody] LogInUserCommand command)
        {


            var result = await mediator.Send(command);


            SetCookie(14, result.RefreshToken);


            var response = ApiResponseFactory.Success(result.Response);


            return Ok(response);

        }


        [Authorize]

        [HttpPost("logout")]



        public async Task<ActionResult<SuccessMessage>> LogOut([FromBody] LogOutRequestDto request)
        {


            var refreshToken = ExtractRefreshToken(Request);



            var command = new LogOutCommand
            {
                RefreshToken = refreshToken,
                DeviceId = request.DeviceId
            };

            var result = await mediator.Send(command);



            Response.Cookies.Delete("refreshToken");

            return Ok(ApiResponseFactory.Success(result));
        }


        [HttpPost("refresh-access")]
        public async Task<ActionResult<SuccessWithData<LogInResponseDto>>> RefreshAccessToken([FromBody] RefreshAccessTokenCommand command)
        {



            command.RefreshToken = ExtractRefreshToken(Request);



            var result = await mediator.Send(command);
            SetCookie(14, result.RefreshToken);

            return Ok(ApiResponseFactory.Success<LogInResponseDto>(result.Response));


        }


        ///vrtCKbkNqQ3/GD8UbaBWyoWHbwDvTZjQ/QzHMAfMlt2XSvRJV/J3PfF0o/Q92v5yMiJGZ4NMWzttO5JKuTz/w==



        private string ExtractRefreshToken(HttpRequest request)
        {

            var refreshToken = request.Cookies["refreshToken"];




            if (string.IsNullOrEmpty(refreshToken))
            {

                throw new UnAuthorizedException("Refresh token missing.");

            }

            return refreshToken;
        }


        private void SetCookie(int days, string token)
        {

            Response.Cookies.Append("refreshToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Only if you're using HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(days)
            });


        }

    }
}
