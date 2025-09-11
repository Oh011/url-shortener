using Project.Application.Common.Interfaces;
using Project.Infrastructure.Persistence;
using Shared.Options;

namespace Url_Shortener.Extensions
{
    public static class WebApplicationExtensions
    {



        public static WebApplication UseShardManager(this WebApplication app, IConfiguration configuration)
        {


            using var scope = app.Services.CreateScope();



            var shardManager = scope.ServiceProvider.GetRequiredService<IShardManager>();

            Console.WriteLine("ShardManager initialized and available everywhere.");







            return app;

        }


        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {


            using var scope = app.Services.CreateScope();

            var initializer = scope.ServiceProvider.GetRequiredService<IdentityInitializer>();


            await initializer.InitializeAsync();


            return app;
        }

        public static WebApplication ApplyShardMigrations(this WebApplication app, IConfiguration configuration)
        {



            var shards = configuration.GetSection("Shards").Get<List<ShardInfo>>() ?? new List<ShardInfo>();

            // Run migrations on all shards (synchronously)
            var migrationRunner = new ShardMigrationRunner(shards);
            migrationRunner.MigrateAllShards();


            return app;

        }
    }
}
