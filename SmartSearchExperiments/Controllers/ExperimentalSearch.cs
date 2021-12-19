using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;

namespace SmartSearchExperiments.Controllers
{
    [ApiController]
    public class ExperimentalSearch : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
