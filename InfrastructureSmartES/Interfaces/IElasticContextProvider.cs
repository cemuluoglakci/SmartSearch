using Nest;

namespace InfrastructureSmartES.Interfaces
{
    public interface IElasticContextProvider
    {
        IElasticClient GetClient();
    }
}
