using ApplicationSmart.CombinedSearch.BasicSearch;
using ApplicationSmart.Interfaces;
using CoreSmart.Entities;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ApplicationSmart.ElasticsearchHelpers
{
    public static class SearchHelper
    {
        public static SearchResultDto GetDocumentDetails (string type, object obj)
        {
            var resultDto = new SearchResultDto
            {
                Type = type,
            };
            var json = JsonConvert.SerializeObject(obj);
            

            if (type=="Property")
            {
                var innerDocument = JsonConvert.DeserializeObject<CoreSmart.Entities.Properties > (json);
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

    }
}
