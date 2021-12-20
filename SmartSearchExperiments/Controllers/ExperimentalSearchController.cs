using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;
using SmartSearchExperiments.Models;
using System.IO;
using Newtonsoft.Json;

namespace SmartSearchExperiments.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExperimentalSearchController : Controller
    {
        private readonly IConfiguration _config;

        public ExperimentalSearchController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Search")]
        public async Task<IActionResult> SearchEsDocument(string input)
        {
            string indexName = "properties_test";
            var endpoint = new System.Uri(_config.GetValue<string>("smartsearch:endpoint"));
            var settings = new ConnectionSettings(endpoint).DefaultIndex(indexName).DefaultFieldNameInferrer(p => p);
            var client = new ElasticClient(settings);



            //client.LowLevel.Index.Indices.Create("properties", index => index.Map<Models.Properties>(x => x.AutoMap()));
            //var response = client.LowLevel.CreateUsingType.<Models.Properties> ("feederapp", json);
            //var searchResponse0 = await client.SearchAsync<PropertiesIndexed>(s => s.Query(q => q.QueryString(d => d.Query('*' + input + '*'))));
            //var searchResponse0 = await client.SearchAsync<PropertiesIndexed>();

            var searchResponse0 = await client.SearchAsync<PropertiesIndexed>(s => s.Source()
                                .Query(q => q
                                .QueryString(qs => qs
                                .AnalyzeWildcard()
                                   .Query(input.ToLower() + "*")
                                   .Fields(fs => fs
                                       .Fields(f1 => f1.Property.Name
                                               //,f2 => f2.FormerName,
                                               //f3 => f3.City,
                                               //f4 => f4.Market
                                               )
                                   )
                                   )));
            //var searchResponse = await client.SearchAsync<Models.Properties>(m => m.Index("properties"));

            //var retrievedData = from opt in searchResponse.Hits select new Models.Properties { 
            //city=opt.Source.city,
            //formerName= opt.Source.formerName,
            //market = opt.Source.market,
            //name = opt.Source.name,
            //propertyID = opt.Source.propertyID,
            //state = opt.Source.state,
            //streetAddress = opt.Source.streetAddress,
            //_id = Convert.ToInt32(opt.Id)
            //};


            input = "casa";
            //var response2 = client.Search<Management>(s => s.Query(q => q.QueryString(d => d.Query('*' + input + '*'))));

            //var response = await client.SearchAsync<Management>(s => s.Query(q => q.QueryString(d => d.Query('*' + input + '*'))));
            return Ok(searchResponse0.Documents);


        }
        [HttpGet("Load2ES")]
        public async Task<IActionResult> Load2ES()
        {
            string fileName = "properties_test.json";
            string indexName = "properties_test";
            List<PropertiesIndexed> items = new List<PropertiesIndexed>();
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<PropertiesIndexed>>(json);
            }

            var endpoint = new System.Uri(_config.GetValue<string>("smartsearch:endpoint"));
            //endpoint = "https://search-smartsearch-25aeejmcjdzwer7ono5jdewkza.us-east-2.es.amazonaws.com";
            var settings = new ConnectionSettings(endpoint).DefaultIndex(indexName).DefaultFieldNameInferrer(p => p);

            var client = new ElasticClient(settings);

            var bulkAllObservable = client.BulkAll(items, b => b
                .Index(indexName)
                .BackOffTime("30s")
                .BackOffRetries(2)
                .RefreshOnCompleted()
                .MaxDegreeOfParallelism(Environment.ProcessorCount)
                .Size(1000)
            )
            .Wait(TimeSpan.FromMinutes(2), next =>
            {
                // do something e.g. write number of pages to console
            });

            return Ok(bulkAllObservable.TotalNumberOfFailedBuffers);

        }

        [HttpGet("DeleteData")]
        public async Task<IActionResult> DeleteData()
        {
            var endpoint = new System.Uri(_config.GetValue<string>("smartsearch:endpoint"));
            //endpoint = "https://search-smartsearch-25aeejmcjdzwer7ono5jdewkza.us-east-2.es.amazonaws.com";
            var settings = new ConnectionSettings(endpoint).DefaultIndex("properties");

            var client = new ElasticClient(settings);
            var resp = await client.DeleteByQueryAsync<Models.Properties>(q => q.MatchAll());



            return Ok(resp.Deleted);
        }

        [HttpGet("CreateIndex")]
        public IActionResult CreateIndex()
        {
            string indexName = "properties_test";

            var endpoint = new System.Uri(_config.GetValue<string>("smartsearch:endpoint"));
            //endpoint = "https://search-smartsearch-25aeejmcjdzwer7ono5jdewkza.us-east-2.es.amazonaws.com";
            var settings = new ConnectionSettings(endpoint).DefaultIndex(indexName).DefaultFieldNameInferrer(p => p); ;
            //        settings.MapPropertiesFor<MyClass>(props => props
            //.Rename(p => p.Foo, "bar")
            //.Ignore(p => p.Bar));

            var client = new ElasticClient(settings);
            
            //var isettings = new IndexSettings();

            //isettings.NumberOfReplicas = 0;
            //isettings.NumberOfShards = 1;
            //isettings.Settings.Add("merge.policy.merge_factor", "10");
            //isettings.Settings.Add("search.slowlog.threshold.fetch.warn", "1s");

            var resp = client.Indices.Create(indexName, c => c
            .Settings(s => s
                .Analysis(a => a
                    .CharFilters(cf => cf
                        .Mapping("remove_punctuations", mca => mca
                            .Mappings(new[]
                            {
                                    "( => ",
                                    ") => ",
                                    "* => ",
                                    ". => ",
                            })
                        )
                    )
                    .Analyzers(an => an
                        .Custom("default", ca => ca
                            .CharFilters("html_strip", "remove_punctuations")
                            .Tokenizer("standard")
                            .Filters("keyword_repeat", "lowercase", "porter_stem", "remove_duplicates", /*"ngram", "synonym",*/ "stop")
                        )
                    )
                )
            )
            .Map<PropertiesIndexed>(mm => mm
                .AutoMap()
                //.Properties(p => p
                //    .Text(t => t
                //        .Name(nm => nm.Property.Name).Analyzer("smart_analyzer")
                //        .Name(nm => nm.Property.FormerName).Analyzer("smart_analyzer")
                //        .Name(nm=>nm.Property.StreetAddress).Analyzer("smart_analyzer")
                //        .Name(nm=>nm.Property.City).Analyzer("smart_analyzer")
                //        .Name(nm=>nm.Property.Market).Analyzer("smart_analyzer")
                //        .Name(nm=>nm.Property.State).Analyzer("smart_analyzer")
                //    )
                //)
            )
        );

            //var resp = client.Indices.Create("properties", i => i.Map<PropertiesIndexed>(x => x.AutoMap()));
            //var resp2 = client.Indices.Create("mgmt", i => i.Map<MgmtIndexed>(x => x.AutoMap()));



            return Ok(resp.Index);
        }

    }
}
