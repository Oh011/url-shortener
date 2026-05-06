using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Identity.Entities;
using UrlShortener.Infrastructure.Migrations.AppDataBaseMigrations;
using UrlShortener.Infrastructure.Persistence.Configurations.AppDb;

namespace UrlShortener.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {




        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {





        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(
       typeof(UserStatisticsConfigurations).Assembly,
       t => t.Namespace == "UrlShortener.Infrastructure.Persistence.Configurations.AppDb");


            //builder.ApplyConfigurationsFromAssembly(typeof(UserStatisticsConfigurations).Assembly);
        }



        public DbSet<RefreshToken> RefershTokens { get; set; }

        public DbSet<UserAnalytics> UserAnalytics { get; set; }
        public DbSet<UserStatistics> UserStatistics { get; set; }
    }
}
