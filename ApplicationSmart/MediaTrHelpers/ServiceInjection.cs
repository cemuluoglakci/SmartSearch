using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ApplicationSmart.MediaTrHelpers
{
    public static class ServiceInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerf<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestVal<,>));
            return services;
        }

    }
}
