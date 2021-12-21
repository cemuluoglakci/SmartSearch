using AutoMapper;

namespace ApplicationSmart.Abstracts
{
    public abstract class Mapper <T>
    {
        void Mapping(Profile profile)
        {
            profile.CreateMap(typeof(T), GetType());
        }

    }
}
