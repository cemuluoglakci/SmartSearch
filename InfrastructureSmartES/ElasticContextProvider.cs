using System;
using System.Linq;
using ApplicationSmart.Interfaces;
using Elasticsearch.Net;
using InfrastructureSmartES.Interfaces;
using Nest;

namespace InfrastructureSmartES
{
    public class ElasticContextProvider : ISmartSearchESContext
    {
        private const string DEFAULT_CONNECTION = "http://localhost:9200/";
        private readonly IElasticConfigurationService _ESconfig;
        private IElasticClient Client { get; set; }

        public ElasticContextProvider(IElasticConfigurationService ESconfigService)
        {
            _ESconfig = ESconfigService;
        }

        public IElasticClient GetClient()
        {
            if (Client == null)
            {
                var hosts = _ESconfig.Get().Addresses ?? new[] { DEFAULT_CONNECTION };
                var settings = new ConnectionSettings(new Uri(hosts[0])).DefaultIndex("properties").DefaultFieldNameInferrer(p => p);
                Client = new ElasticClient(settings);
            }
            return Client;
        }
    }
}
