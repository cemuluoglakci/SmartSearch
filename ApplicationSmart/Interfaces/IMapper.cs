using AutoMapper;

namespace ApplicationSmart.Interfaces
{
    public interface IMapper <T>
    {
        void Mapping(Profile profile);
    }
}
