using Microsoft.OpenApi.Models;
using Project.Application.DependencyInjection;
using Project.Infrastructure.DependencyInjection;
using Shared.Options;
using System.Reflection;
using Url_Shortener.Extensions;
using Url_Shortener.Middlewares;

namespace Url_Shortener
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);





            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddPresentationServices();

            builder.Services.AddInfrastructure(builder
                .Configuration);


            builder.Services.AddCustomRateLimiting();

            builder.Services.AddApplication();



            builder.Services.Configure<ShortenerOptions>(builder.Configuration.GetSection("shortener"));



            builder.Services.AddSwaggerGen(c =>
            {
                // Include XML comments (see next step)
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // Add JWT Auth support
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token in the format **Bearer YOUR_TOKEN**"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });


            //var shards = builder.Configuration.GetSection("Shards").Get<List<ShardInfo>>() ?? new List<ShardInfo>();

            //// Run migrations on all shards (synchronously)
            //var migrationRunner = new ShardMigrationRunner(shards);
            //migrationRunner.MigrateAllShards();

            var app = builder.Build();


            app.ApplyShardMigrations(builder.Configuration);

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }
            await app.SeedDbAsync();

            app.UseRateLimiter();


            app.UseShardManager(builder.Configuration);

            app.UseHttpsRedirection();


            app.UseAuthentication(); //--> here JWT bearer handler is invoked.

            app.UseAuthorization();






            app.MapControllers();

            app.Run();
        }
    }
}
