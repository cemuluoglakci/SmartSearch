using ApplicationSmart.Interfaces;
using AutoMapper;
using CoreSmart.Entities;
using System.Collections.Generic;
using System.Text;

namespace ApplicationSmart.Property.Queries.GetPropertiesList
{
    public class PropertiesListVm /*: IMapper<PropertiesIndexed>*/
    {
        public IList<PropertyLookUpDto> Properties { get; set; }

    //    public void Mapping(Profile profile)
    //    {
    //        profile.CreateMap<List<PropertiesIndexed>, PropertiesListVm.Properties>()
    //.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Property.PropertyID))
    //.ForMember(d => d.Name, opt => opt.MapFrom(s => s.Property.Name));
    //    }
    }
}
