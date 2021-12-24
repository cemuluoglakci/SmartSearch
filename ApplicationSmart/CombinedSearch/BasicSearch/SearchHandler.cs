using ApplicationSmart.ElasticsearchHelpers;
using ApplicationSmart.CombinedSearch;
using ApplicationSmart.Interfaces;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationSmart.CombinedSearch.BasicSearch
{
    public class SearchHandler : IRequestHandler<SearchInput, SearchResultListVm>
    {
        private readonly ISmartSearchESContext _ESContext;
        private readonly IMapper _mapper;

        public SearchHandler(IMapper mapper, ISmartSearchESContext ESContext)
        {
            _ESContext = ESContext;
            _mapper = mapper;
        }

        public async Task<SearchResultListVm> Handle(SearchInput request, CancellationToken cancellationToken)
        {
            var client = _ESContext.GetClient();

            var Index1 = "properties";
            var Index2 = "mgmt";

            var testList = new[] { "DFW" };

            var searchResponse = await client.SearchAsync<dynamic>(s => s
                .From(0)
                .Size(request.Limit)
                .AllIndices()
                .Query(q => (q
                   .QueryString(t => t
                       .Query(request.SearchPhrase))
                    && +q.Terms(t => t
                        .Field("_index")
                        .Terms(new[] { Index1, Index2 })
                        )
                    )
                )
            );
            
            var ResultList = searchResponse.Documents.ToList();
            var ResultDtoList = new List<SearchResultDto>();
            foreach (Dictionary<string, object> ResultDict in ResultList)
            {
                ResultDtoList.Add(SearchHelper.GetDocumentDetails(ResultDict.First().Key, ResultDict.First().Value));
            }

            var vm = new SearchResultListVm
            {
                Results = (IList<SearchResultDto>)ResultDtoList
            };
            return vm;
        }
    }
}
