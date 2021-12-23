using ApplicationSmart.ElasticsearchHelpers;
using ApplicationSmart.Interfaces;
using AutoMapper;
using CoreSmart.Entities;
using MediatR;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationSmart.Combined
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
                       //Check if default analyzer working
                       .Query(request.SearchPhrase))
                    && +q.Terms(t => t
                        .Field("_index")
                        .Terms(new[] { Index1, Index2 })
                        )
                     && +q.Bool(bq => bq
                        .Filter(
                            //fq => fq.Terms(t => t.Field("Market").Terms(testList)),
                            fq => fq.Terms(t => t.Field("State").Terms(request.StateList)))
                        )
                    )
                )
            );
            //var ResultDtoList = searchResponse.Documents.ToList().Select(ResultDict => SearchHelper.GetDocumentDetails(ResultDict.First().Key, ResultDict.First().Value)).ToList();

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
