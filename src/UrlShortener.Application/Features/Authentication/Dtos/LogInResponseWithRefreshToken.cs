namespace Project.Application.Features.Authentication.Dtos
{
    public class LogInResponseWithRefreshToken
    {

        public LogInResponseDto Response { get; set; } = default!;


        public string RefreshToken { get; set; }



    }
}
