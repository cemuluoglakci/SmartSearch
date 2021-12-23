using ApplicationSmart.Interfaces;
using AutoMapper;
using CoreSmart.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationSmart.Combined
{
    public class SearchResultDto : IMapper<Dictionary<string, string>>
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }

        public static SearchResultDto GetDocumentDetails(string type, object obj)
        {
            var resultDto = new SearchResultDto
            {
                Type = type,
            };
            var json = JsonConvert.SerializeObject(obj);


            if (type == "Property")
            {
                var innerDocument = JsonConvert.DeserializeObject<CoreSmart.Entities.Properties>(json);
                resultDto.Id = innerDocument.PropertyID;
                resultDto.Name = innerDocument.Name;
                resultDto.Market = innerDocument.Market;
            }
            else if (type == "Mgmt")
            {
                Mgmt innerDocument = JsonConvert.DeserializeObject<Mgmt>(json);
                resultDto.Id = innerDocument.MgmtID;
                resultDto.Name = innerDocument.Name;
                resultDto.Market = innerDocument.Market;
            }
            return resultDto;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Array, SearchResultDto>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Length.ToString())

                );
        }
    }
}
