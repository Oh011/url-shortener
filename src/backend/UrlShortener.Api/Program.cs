using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Shared.Options;
using System.Reflection;
using Url_Shortener.Extensions;
using Url_Shortener.Middlewares;
using UrlShortener.Application.DependencyInjection;
using UrlShortener.Infrastructure.DependencyInjection;

namespace Url_Shortener
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);





            builder.Services.AddEndpointsApiExplorer();
        

            builder.Services.AddPresentationServices();

            builder.Services.AddApplication();

            builder.Services.AddInfrastructure(builder
                .Configuration);


            builder.Services.AddCustomRateLimiting();


   


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



            var app = builder.Build();


            app.ApplyShardMigrations(builder.Configuration);

            app.UseMiddleware<GlobalErrorHandlingMiddleware>();


            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Url Shortener API V1");
                    c.RoutePrefix = "swagger";
                });

                //app.MapScalarApiReference(options =>
                //{
                //    options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
                //});

            }




            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.Path == "/")
                {
                    ctx.Response.Redirect("/swagger");
                    return;
                }

                await next();
            });


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
