using ApplicationSmart.Abstracts;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationSmart.Property.Queries.GetPropertiesList
{
    public class PropertyLookUpDto: Mapper<CoreSmart.Entities.Properties>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CoreSmart.Entities.Properties, PropertyLookUpDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.PropertyID))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));
        }


    }
}
