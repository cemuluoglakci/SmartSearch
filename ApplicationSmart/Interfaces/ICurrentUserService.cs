
namespace ApplicationSmart.Interfaces
{
    public interface ICurrentUserService
    {
        string ID { get; }

        bool IsAuth { get; }
    }
}
