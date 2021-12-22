using ApplicationSmart.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoreSmart.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        //private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public GetPropertiesListQueryHandler(IMapper mapper, ISmartSearchESContext ESContext)
        {
            _ESContext = ESContext;
            _mapper = mapper;
        }
        public async Task<PropertiesListVm> Handle(GetPropertiesListQuery request, CancellationToken cancellationToken)
        {
            //var input = "casa";
            //string indexName = "properties_test";
            //var endpoint = new System.Uri(_config.GetValue<string>("smartsearch:endpoint"));
            //var settings = new ConnectionSettings(endpoint).DefaultIndex(indexName).DefaultFieldNameInferrer(p => p);
            //var client = new ElasticClient(settings);
            var client = _ESContext.GetClient();



            //client.LowLevel.Index.Indices.Create("properties", index => index.Map<Models.Properties>(x => x.AutoMap()));
            //var response = client.LowLevel.CreateUsingType.<Models.Properties> ("feederapp", json);
            //var searchResponse0 = await client.SearchAsync<PropertiesIndexed>(s => s.Query(q => q.QueryString(d => d.Query('*' + input + '*'))));
            //var searchResponse0 = await client.SearchAsync<PropertiesIndexed>();

            var searchResponse = await client.SearchAsync<PropertiesIndexed>(m => m.Index("properties_test"));

            var PropertyList = searchResponse.Documents.ToList();

            var testProperty = PropertyList[0];


            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<PropertiesIndexed, PropertyLookUpDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Property.PropertyID))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Property.Name));
            });
            IMapper mapper = config.CreateMapper();
            var dest = mapper.Map<PropertiesIndexed, PropertyLookUpDto>(testProperty);



            var testLookup = _mapper.Map<PropertiesIndexed, PropertyLookUpDto>(testProperty);
            //List<PropertyLookUpDto> Views = new List<PropertyLookUpDto>();
            var PropertyLookupList = _mapper.Map<List<PropertiesIndexed>, List<PropertyLookUpDto>>(PropertyList);

            //var test = await client.SearchAsync<PropertiesIndexed>();
            //.ProjectTo<PropertyLookUpDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken); ;
            //(s => s.Source()
            //                .Query(q => q
            //                .QueryString(qs => qs
            //                .AnalyzeWildcard()
            //                   .Query(input.ToLower() + "*")
            //                   .Fields(fs => fs
            //                       .Fields(f1 => f1.Property.Name
            //                               //,f2 => f2.FormerName,
            //                               //f3 => f3.City,
            //                               //f4 => f4.Market
            //                               )
            //                   )
            //                   )));
            var test2 = searchResponse.Documents;
            var vm = new PropertiesListVm
            {
                Properties = PropertyLookupList,
            };
            return vm;
        }
    }

    //public class GetPropertiesListQueryHandler : IRequestHandler<GetPropertiesListQuery, PropertiesListVm>
    //{
    //    private readonly ISmartSearchDataContext _context;
    //    //private readonly IElasticClient _elasticClient;
    //    private readonly IMapper _mapper;

    //    public GetPropertiesListQueryHandler(ISmartSearchDataContext context, IMapper mapper)
    //    {
    //        //_context = context;
    //        _context = context;
    //        _mapper = mapper;
    //    }

    //    public async Task<PropertiesListVm> Handle(GetPropertiesListQuery request, CancellationToken cancellationToken)
    //    {
    //        var properties = await _context.Properties
    //            .ProjectTo<PropertyLookUpDto>(_mapper.ConfigurationProvider)
    //            .OrderBy(i => i.Name)
    //            .ToListAsync(cancellationToken);

    //        var vm = new PropertiesListVm
    //        {
    //            Properties = properties,
    //        };
    //        return vm;
    //    }
    //}
}
