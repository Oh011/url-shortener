using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities;
using Project.Infrastructure.Identity.Entities;
using Project.Infrastructure.Persistence.Configurations.AppDb;

namespace Project.Infrastructure.Persistence.Context
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
       t => t.Namespace == "Project.Infrastructure.Persistence.Configurations.AppDb");


            //builder.ApplyConfigurationsFromAssembly(typeof(UserStatisticsConfigurations).Assembly);
        }



        public DbSet<UserAnalytics> UserAnalytics { get; set; }
        public DbSet<UserStatistics> UserStatistics { get; set; }
    }
}
