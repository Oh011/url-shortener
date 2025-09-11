using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Project.Application.Common.Factories.Interfaces;
using Project.Application.Common.Interfaces;
using Project.Application.Common.Interfaces.Services;
using Project.Application.Features.Authentication.Interfaces;
using Project.Infrastructure.Identity.DataSeeding;
using Project.Infrastructure.Identity.Entities;
using Project.Infrastructure.Identity.Services;
using Project.Infrastructure.Persistence;
using Project.Infrastructure.Persistence.Context;
using Project.Infrastructure.Persistence.Repositories;
using Project.Infrastructure.Services;
using Project.Infrastructure.Services.Factories;
using Shared.Options;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text;
using AuthenticationService = Project.Infrastructure.Identity.Services.AuthenticationService;
using IAuthenticationService = Project.Application.Features.Authentication.Interfaces.IAuthenticationService;




namespace Project.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {


        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddScoped<IUserContextService, UserContextService>();

            services.AddScoped<ICacheRepository, CacheRepository>();


            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {

                var configurations = ConfigurationOptions.Parse(configuration.GetSection("ConnectionStrings")["Redis"], true);
                configurations.ResolveDns = true;

                try
                {

                    return ConnectionMultiplexer.Connect(configurations);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Could not connect to Redis");
                    throw;
                }

            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {

                options.UseSqlServer(configuration.GetSection("ConnectionStrings")["DefaultConnectionString"]);
            });

            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();

            services.Configure<List<ShardInfo>>(configuration.GetSection("Shards"));


            var shards = configuration.GetSection("Shards").Get<List<ShardInfo>>() ?? new List<ShardInfo>();

            // Register as singleton so it's reused everywhere
            services.AddSingleton<IShardManager>(new ShardManager(shards));



            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(configuration.GetSection("ConnectionStrings")["DefaultConnectionString"]);
            });

            services.AddHangfireServer();


            services.AddScoped<IUserStatisticsService, UserStatisticsService>();

            services.AddScoped<IBackgroundJobService, HangFireBackgroundJobService>();


            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IUniqueIdGenerator, SqlServerSequenceIdGenerator>();

            services.AddScoped<IUserAnalyticsService, UserAnalyticsService>();
            services.AddScoped<IUrlAccessLogService, UrlAccessLogService>();

            services.AddScoped<IdentityInitializer, IdentityDbInitializer>();


            services.Configure<JwtOptions>(configuration.GetSection("jwtOptions"));

            services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));

            services.AddScoped<ITokenService, TokenService>();

            ConfigureIdentity(services);


            ConfigureJwtOptions(services, configuration);




            return services;
        }



        private static void ConfigureJwtOptions(this IServiceCollection services, IConfiguration configuration)
        {


            var jwtOptions = configuration.GetSection("jwtOptions").Get<JwtOptions>();


            services.AddAuthentication(options =>
            {


                //{How to check if a user is logged in}
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                //{How to respond to unauthenticated requests}
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }



            ).AddJwtBearer(options =>
            {

                //These events run inside the middleware pipeline, specifically during 
                //UseAuthentication(), before the request reaches your controllers or hubs.



                options.Events = new JwtBearerEvents
                {


                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        // prevent redirect to login page
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Unauthorized" });
                        return context.Response.WriteAsync(result);
                    },


                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our SignalR hub
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/hubs/notifications")) // make sure this matches your actual hub route
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }

                };

                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,


                    RoleClaimType = ClaimTypes.Role,


                    ValidAudience = jwtOptions.Audiance,

                    ValidIssuer = jwtOptions.Issuer,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    ClockSkew = TimeSpan.Zero,
                };
            });

        }

        private static void ConfigureIdentity(this IServiceCollection services)
        {



            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;

                options.Tokens.EmailConfirmationTokenProvider = "Default";

                options.SignIn.RequireConfirmedEmail = true;

                // Lockout settings (optional for security)
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;

                // Sign-in settings




            }).AddEntityFrameworkStores<ApplicationDbContext>()
         .AddDefaultTokenProviders();



            //---> :
            //Registers services like:

            //UserManager < ApplicationUser > → to manage users(create, update, delete, passwords, etc.).

            //SignInManager < ApplicationUser > → to handle login / sign -in logic.

            //RoleManager < IdentityRole > → to manage roles.

            //Token providers(EmailConfirmationTokenProvider, etc.).

        }
    }


}

