namespace Project.Application.Features.Authentication.Dtos
{
    public class LogInResponseDto
    {

        public string UserId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string AccessToken { get; set; }
    }
}
