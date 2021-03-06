using ApplicationSmart.Interfaces;
using AutoMapper;
using CoreSmart.Entities;
using MediatR;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationSmart.Property.Queries.GetPropertiesList
{
    public class GetPropertiesListQueryHandler : IRequestHandler<GetPropertiesListQuery, PropertiesListVm>
    {
        private readonly ISmartSearchESContext _ESContext;
        private readonly IMapper _mapper;

        public GetPropertiesListQueryHandler(IMapper mapper, ISmartSearchESContext ESContext)
        {
            _ESContext = ESContext;
            _mapper = mapper;
        }

        public async Task<PropertiesListVm> Handle(GetPropertiesListQuery request, CancellationToken cancellationToken)
        {
            var client = _ESContext.GetClient();

            var searchResponse = await client.SearchAsync<PropertiesIndexed>(m => m.Index("properties"));
            //var searchResponse = await client.SearchAsync<PropertiesIndexed>(m => m
            //    .Query(q => q
            //    .Bool(b=>b
            //    .Must(must=>must

            //    .Match(qs => qs
            //    .Field(f=>f.Property.Market)
            //    .Query("Atlanta"))

            //))));

            var PropertyList = searchResponse.Documents.ToList();
            var PropertyLookupList = _mapper.Map<List<PropertiesIndexed>, List<PropertyLookUpDto>>(PropertyList);
            var vm = new PropertiesListVm
            {
                Properties = PropertyLookupList,
            };
            return vm;
        }
    }
}
