using System.Threading.RateLimiting;

namespace Url_Shortener.Extensions
{
    public static class RateLimiterExtensions
    {


        public static IServiceCollection AddCustomRateLimiting(this IServiceCollection services)
        {

            //--> Registers the built-in ASP.NET Rate Limiter middleware
            services.AddRateLimiter(options =>
            {

                //--> Creates a named policy you can attach to endpoints ([EnableRateLimiting("LoginPolicy")]).
                options.AddPolicy("LogInPolicy", context => RateLimitPartition.GetTokenBucketLimiter(

                    context.Connection.RemoteIpAddress?.ToString() ?? "ann",


                    _ => new TokenBucketRateLimiterOptions
                    {

                        TokenLimit = 5,
                        TokensPerPeriod = 1,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                        QueueLimit = 0, // don’t queue, reject immediately
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        AutoReplenishment = true

                    }
                ));


                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ =>


                    RateLimitPartition.GetTokenBucketLimiter("global", _ => new TokenBucketRateLimiterOptions
                    {

                        TokenLimit = 100,
                        TokensPerPeriod = 50,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(1),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        AutoReplenishment = true
                    }));


                options.RejectionStatusCode = 429;

            });

            return services;
        }
    }

}
