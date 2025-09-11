using Microsoft.AspNetCore.Identity;
using Project.Domain.Entities;

namespace Project.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {






        public UserStatistics? Statistics { get; set; }

        public ICollection<UserAnalytics> UserAnalytics { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
