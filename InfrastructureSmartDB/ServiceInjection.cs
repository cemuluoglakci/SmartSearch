using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace InfrastructureSmartDB
{
    public static class ServiceInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SmartSearchDataContext>(options =>
                options.UseMySQL(configuration.GetConnectionString("SmartSearchDatabase")));

            services.AddScoped<SmartSearchDataContext>(provider => provider.GetService<SmartSearchDataContext>());

            return services;
        }


    }
}
