using System.Text.Json.Serialization;
using Url_Shortener.ResponseFactories;

namespace Url_Shortener.Extensions
{
    public static class PresentationServiceExtensions
    {


        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {


            services.AddControllers()
                .AddJsonOptions(options =>
                {

                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
                }).ConfigureApiBehaviorOptions(options =>
                {

                    options.InvalidModelStateResponseFactory = ValidationResponseFactory.CustomValidationResponse;
                });

            return services;
        }
    }
}
