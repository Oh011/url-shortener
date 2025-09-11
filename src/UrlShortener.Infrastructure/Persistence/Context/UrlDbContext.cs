using Microsoft.EntityFrameworkCore;
using Project.Domain.Entities;
using Project.Infrastructure.Persistence.Configurations.UrlDb;

namespace Project.Infrastructure.Persistence.Context
{
    public class UrlDbContext : DbContext
    {



        public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options) { }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.ApplyConfigurationsFromAssembly(
typeof(UrlConfigurations).Assembly,
t => t.Namespace == "Project.Infrastructure.Persistence.Configurations.UrlDb");
        }


        public DbSet<Url> Urls { get; set; }
        public DbSet<UrlAccessLog> UrlAccessLogs { get; set; }
    }
}
