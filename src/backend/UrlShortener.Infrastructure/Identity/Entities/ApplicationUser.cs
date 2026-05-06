using Microsoft.AspNetCore.Identity;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {






        public UserStatistics? Statistics { get; set; }

        public ICollection<UserAnalytics> UserAnalytics { get; set; }=new HashSet<UserAnalytics>();
        public ICollection<RefreshToken> RefreshTokens { get; set; }=new HashSet<RefreshToken>();
    }
}
