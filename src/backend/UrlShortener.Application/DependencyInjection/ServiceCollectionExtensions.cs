using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Common.Behaviors;
using UrlShortener.Application.Common.Interfaces;
using UrlShortener.Application.Common.Services.Implementations;
using UrlShortener.Application.Features;

namespace UrlShortener.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {


        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IShortCodeGenerator, ShortCodeGenerator>();

            services.AddMediatR(T =>
            {


                T.RegisterServicesFromAssemblies(typeof(MediatRAssemblyReference).Assembly);
            });


            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

    }
}
