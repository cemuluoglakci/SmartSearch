using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureSmartES.Interfaces
{
    public interface IElasticConfigurationService
    {
        ElasticConfiguration Get();
    }

    public class ElasticConfiguration
    {
        public string[] Addresses { get; set; }
    }
}
