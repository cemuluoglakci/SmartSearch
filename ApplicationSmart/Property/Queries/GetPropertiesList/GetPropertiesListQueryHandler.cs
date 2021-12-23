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
            var testList = new[] { "DFW" };
            var searchResponse = await client.SearchAsync<PropertiesIndexed>(m => m.Index("properties").Query(q => q.Bool(bq => bq
                         .Filter(
                             fq => fq.Terms(t => t.Field("Property.Market").Terms(testList))
                             
                        )
                    )
                )
            );


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
