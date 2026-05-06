using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Persistence.Configurations.UrlDb;

namespace UrlShortener.Infrastructure.Persistence.Context
{
    public class UrlDbContext : DbContext
    {



        public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options) { }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.ApplyConfigurationsFromAssembly(
typeof(UrlConfigurations).Assembly,
t => t.Namespace == "UrlShortener.Infrastructure.Persistence.Configurations.UrlDb");
        }


        public DbSet<Url> Urls { get; set; }
        public DbSet<UrlAccessLog> UrlAccessLogs { get; set; }
    }
}
