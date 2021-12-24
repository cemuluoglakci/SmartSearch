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
            var endpoint = new System.Uri("https://search-smartsearch-25aeejmcjdzwer7ono5jdewkza.us-east-2.es.amazonaws.com");
            var settings = new ConnectionSettings(endpoint).DefaultIndex(indexName).DefaultFieldNameInferrer(p => p);
            var client = new ElasticClient(settings);

            var searchResponse0 = await client.SearchAsync<PropertiesIndexed>(s => s.Source()
                                .Query(q => q
                                .QueryString(qs => qs
                                .AnalyzeWildcard()
                                   .Query(input.ToLower() + "*")
                                   .Fields(fs => fs
                                       .Fields(f1 => f1.Property.Name

                                               )
                                   )
                                   )));
            
            return Ok(searchResponse0.Documents);


        }
        [HttpGet("LoadProperties")]
        public async Task<IActionResult> LoadProperties()
        {
            string fileName = "properties.json";
            string indexName = "properties";
            List<PropertiesIndexed> items = new List<PropertiesIndexed>();
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<PropertiesIndexed>>(json);
            }

            var endpoint =  new System.Uri("https://search-smartsearch-25aeejmcjdzwer7ono5jdewkza.us-east-2.es.amazonaws.com");
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
                .Wait(TimeSpan.FromMinutes(2), next => { });

            return Ok(bulkAllObservable.TotalNumberOfFailedBuffers);
        }


        [HttpGet("LoadMgmt")]
        public IActionResult LoadMgmt()
        {
            string fileName = "mgmt.json";
            string indexName = "mgmt";
            List<MgmtIndexed> items = new List<MgmtIndexed>();
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<MgmtIndexed>>(json);
            }
            var endpoint = new System.Uri("https://search-smartsearch-25aeejmcjdzwer7ono5jdewkza.us-east-2.es.amazonaws.com");
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
                .Wait(TimeSpan.FromMinutes(2), next =>{});

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

        [HttpGet("CreateMgmtIndex")]
        public IActionResult CreateIndex()
        {
            string indexName = "mgmt";

            //var endpoint = _config.GetValue<string>("Elastic:Addresses");
            var endpoint = "https://search-smartsearch-25aeejmcjdzwer7ono5jdewkza.us-east-2.es.amazonaws.com";
            var settings = new ConnectionSettings(new System.Uri(endpoint)).DefaultIndex(indexName).DefaultFieldNameInferrer(p => p); 
            var client = new ElasticClient(settings);
            
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
            .Map<MgmtIndexed>(mm => mm
                .Properties(ps => ps
                    .Object<Models.Mgmt>(o => o
                    .Name(n => n.Mgmt)
                    .Properties(eps => eps
                        .Text(s => s
                            .Name(n => n.Name)
                            .Analyzer("default")
                        )
                        .Keyword(s => s
                            .Name(n => n.Market)
                        )
                        .Keyword(s => s
                            .Name(n => n.State)
                        )
                        .Number(d => d
                            .Name(n => n.MgmtID)
                            .Type(NumberType.Integer)
                        )
                    )
                )
            )
        )
        );

            return Ok(resp.Index);
        }

        [HttpGet("CreatePropertyIndex")]
        public IActionResult CreatePropertyIndex()
        {
            string indexName = "properties";
            var endpoint = "https://search-smartsearch-25aeejmcjdzwer7ono5jdewkza.us-east-2.es.amazonaws.com";
            var settings = new ConnectionSettings(new System.Uri(endpoint)).DefaultIndex(indexName).DefaultFieldNameInferrer(p => p);
            var client = new ElasticClient(settings);

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
                .Properties(ps => ps
                    .Object<Models.Properties>(o => o
                    .Name(n => n.Property)
                    .Properties(eps => eps
                        .Text(s =>s
                            .Name(n => n.Name)
                            //.Analyzer("default")
                        )
                        .Text(s => s
                            .Name(n => n.City)   
                        )
                        .Keyword(s => s
                            .Name(n => n.Market)
                        )
                        .Keyword(s => s
                            .Name(n => n.State)
                        )
                        .Text(s => s
                            .Name(n => n.FormerName)
                        )
                        .Text(s => s
                            .Name(n => n.StreetAddress)
                        )
                        .Number(d => d
                            .Name(n => n.PropertyID)
                            .Type(NumberType.Integer)
                        )
                        .Number(d => d
                            .Name(n => n.Lat)
                            .Type(NumberType.Float)
                        )
                        .Number(d => d
                            .Name(n => n.Lng)
                            .Type(NumberType.Float)
                        )
                    )
                )
            )
        )
        );

            //var resp = client.Indices.Create("properties", i => i.Map<PropertiesIndexed>(x => x.AutoMap()));
            //var resp2 = client.Indices.Create("mgmt", i => i.Map<MgmtIndexed>(x => x.AutoMap()));



            return Ok(resp.Index);
        }

    }
}
