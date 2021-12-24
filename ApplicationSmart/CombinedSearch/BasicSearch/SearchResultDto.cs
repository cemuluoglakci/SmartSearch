using ApplicationSmart.Interfaces;
using AutoMapper;
using CoreSmart.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationSmart.CombinedSearch.BasicSearch
{
    public class SearchResultDto : IMapper<Dictionary<string, string>>
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Array, SearchResultDto>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Length.ToString())

                );
        }
    }
}
