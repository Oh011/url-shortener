using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UrlShortener.Application.Abstrcations.Identity;

namespace UrlShortener.Infrastructure.Services
{
    internal class UserContextService : IUserContextService
    {


        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetCurrentUserId()
        {
            // Access HttpContext from the accessor
            return _httpContextAccessor.HttpContext?.User
                   .FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? null;
        }

    }
}
