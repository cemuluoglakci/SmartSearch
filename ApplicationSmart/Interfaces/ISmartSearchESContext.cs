using Nest;


namespace ApplicationSmart.Interfaces
{
    public interface ISmartSearchESContext
    {
        IElasticClient GetClient();
    }
}
