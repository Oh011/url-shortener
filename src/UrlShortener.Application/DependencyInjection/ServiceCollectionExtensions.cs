using Microsoft.Extensions.DependencyInjection;
using Project.Application.Features;

namespace Project.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {


        public static IServiceCollection AddApplication(this IServiceCollection services)
        {


            services.AddMediatR(T =>
            {


                T.RegisterServicesFromAssemblies(typeof(MediatRAssemblyReference).Assembly);
            });

            return services;
        }

    }
}
