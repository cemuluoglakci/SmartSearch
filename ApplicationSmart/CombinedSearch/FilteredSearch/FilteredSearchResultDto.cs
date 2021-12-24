using ApplicationSmart.Interfaces;
using AutoMapper;
using CoreSmart.Entities;
using CoreSmart.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationSmart.CombinedSearch.FilteredSearch
{
    public class FilteredSearchResultDto : IMapper<DocumentNested>
    {
        public string Index { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }
        public string State { get; set; }



        public void Mapping(Profile profile)
        {
            profile.CreateMap<DocumentNested, FilteredSearchResultDto>()
                .ForMember(d => d.Index, opt => opt.MapFrom(s => s.Property !=null ? s.Property.ToString() : s.Mgmt.ToString()))
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Property !=null ? s.Property.PropertyID : s.Mgmt.MgmtID))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Property !=null ? s.Property.Name : s.Mgmt.Name))
                .ForMember(d => d.Market, opt => opt.MapFrom(s => s.Property !=null ? s.Property.Market : s.Mgmt.Market))
                .ForMember(d => d.State, opt => opt.MapFrom(s => s.Property !=null ? s.Property.State : s.Mgmt.State));
        }

    }


    public class DocumentNested
    {
        public CoreSmart.Entities.Properties Property { get; set; }
        public Mgmt Mgmt { get; set; }

    }


}
