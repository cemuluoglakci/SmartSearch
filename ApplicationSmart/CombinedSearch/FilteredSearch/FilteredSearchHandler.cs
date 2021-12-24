using ApplicationSmart.ElasticsearchHelpers;
using ApplicationSmart.Interfaces;
using AutoMapper;
using CoreSmart.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationSmart.CombinedSearch.FilteredSearch
{
    public class FilteredSearchHandler : IRequestHandler<FilteredSearchInput, FilteredSearchResultListVm>
    {
        private readonly ISmartSearchESContext _ESContext;
        private readonly IMapper _mapper;

        public FilteredSearchHandler(IMapper mapper, ISmartSearchESContext ESContext)
        {
            _ESContext = ESContext;
            _mapper = mapper;
        }

        public async Task<FilteredSearchResultListVm> Handle(FilteredSearchInput request, CancellationToken cancellationToken)
        {
            var client = _ESContext.GetClient();

            var Index1 = "properties";
            var Index2 = "mgmt";

            var searchResponse = await client.SearchAsync<DocumentNested>(s => s
                .AllIndices()
                .Size(request.Limit)
                .Query(q => (q
                       .QueryString(t => t
                         .Query(request.SearchPhrase + "*")
                           )
                    && +q.Terms(t => t
                        .Field("_index")
                        .Terms(new[] { Index1, Index2 })
                        )

                && +q
                .Bool(bq => bq
                   .Filter(
                       fq => fq.Terms(t => t.Field(f => f.Mgmt.Market).Terms(request.MarketList)),
                       fq => fq.Terms(t => t.Field(f => f.Mgmt.State).Terms(request.StateList))
                       )
                   )
                || +q.Bool(bq => bq
                   .Filter(
                       fq => fq.Terms(t => t.Field(f => f.Property.Market).Terms(request.MarketList)),
                       fq => fq.Terms(t => t.Field(f => f.Property.State).Terms(request.StateList))
                       )
                   )
                )
            )
            );
            
            var ResultList = searchResponse.Documents.ToList();
            var ResultDtoList = _mapper.Map<List<FilteredSearchResultDto>>(ResultList);

            var vm = new FilteredSearchResultListVm
            {
                Results = ResultDtoList
            };
            return vm;
        }
    }
}
