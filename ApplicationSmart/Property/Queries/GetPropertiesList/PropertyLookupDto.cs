using ApplicationSmart.Interfaces;
using AutoMapper;
using CoreSmart.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationSmart.Property.Queries.GetPropertiesList
{
    public class PropertyLookUpDto : IMapper<PropertiesIndexed>
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PropertiesIndexed, PropertyLookUpDto>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Property.ToString()))
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Property.PropertyID))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Property.Name));
        }
    }
}
