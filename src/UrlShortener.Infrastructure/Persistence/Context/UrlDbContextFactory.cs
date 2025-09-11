using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Project.Infrastructure.Persistence.Context
{
    internal class UrlDbContextFactory : IDesignTimeDbContextFactory<UrlDbContext>
    {
        public UrlDbContext CreateDbContext(string[] args)
        {


            var optionsBuilder = new DbContextOptionsBuilder<UrlDbContext>();

            // For migrations, just pick ONE shard connection string
            // (others will get migrated later at runtime with your ShardMigrationRunner)
            var connectionString = "Host=localhost;Port=5433;Database=urls;Username=admin;Password=admin";

            optionsBuilder.UseNpgsql(connectionString);

            return new UrlDbContext(optionsBuilder.Options);
        }
    }
}
