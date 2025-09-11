using Microsoft.EntityFrameworkCore;
using Project.Infrastructure.Persistence.Context;
using Shared.Options;

namespace Project.Infrastructure.Persistence
{


    public class ShardMigrationRunner
    {
        private readonly List<ShardInfo> _shards;

        public ShardMigrationRunner(List<ShardInfo> shards)
        {
            _shards = shards ?? throw new ArgumentNullException(nameof(shards));
        }

        public void MigrateAllShards()
        {
            foreach (var shard in _shards)
            {
                Console.WriteLine($"Migrating shard: {shard.Name}");

                var options = new DbContextOptionsBuilder<UrlDbContext>()
                    .UseNpgsql(shard.ConnectionString)
                    .Options;

                using var context = new UrlDbContext(options);


                try
                {

                    var pending = context.Database.GetPendingMigrations().ToList();
                    if (pending.Any())
                    {
                        Console.WriteLine($"Applying {pending.Count} migrations to {shard.Name}...");
                        context.Database.Migrate();
                        Console.WriteLine($"Shard {shard.Name} migrated successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Shard {shard.Name} is already up to date.");
                    }

                }

                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message.ToString());

                }

            }
        }

        public async Task MigrateAllShardsAsync()
        {
            foreach (var shard in _shards)
            {
                Console.WriteLine($"Migrating shard: {shard.Name}");

                var options = new DbContextOptionsBuilder<UrlDbContext>()
                    .UseNpgsql(shard.ConnectionString)
                    .Options;

                await using var context = new UrlDbContext(options);

                var pending = (await context.Database.GetPendingMigrationsAsync()).ToList();

                if (pending.Any())
                {
                    Console.WriteLine($"Applying {pending.Count} migrations to {shard.Name}...");
                    await context.Database.MigrateAsync();
                    Console.WriteLine($"Shard {shard.Name} migrated successfully.");
                }
                else
                {
                    Console.WriteLine($"Shard {shard.Name} is already up to date.");
                }
            }
        }
    }


}