using ApplicationSmart.Interfaces;
using InfrastructureSmartES.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfrastructureSmartES
{
    public static class ServiceInjection
    {

        public static void AddESInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<ISmartSearchESContext, ElasticContextProvider>();
            services.AddTransient<IElasticConfigurationService, ElasticConfigurationService>();
        }


    }
}
