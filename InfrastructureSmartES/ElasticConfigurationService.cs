using InfrastructureSmartES.Interfaces;
using Microsoft.Extensions.Configuration;

namespace InfrastructureSmartES
{
    public class ElasticConfigurationService : IElasticConfigurationService
    {
        private readonly IConfiguration _configuration;

        public ElasticConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ElasticConfiguration Get()
        {
            return _configuration.GetSection("Elastic").Get<ElasticConfiguration>();
        }
    }
}
