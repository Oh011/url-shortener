using Microsoft.AspNetCore.Http;
using Project.Application.Common.Interfaces.Services;
using System.Security.Claims;

namespace Project.Infrastructure.Services
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
