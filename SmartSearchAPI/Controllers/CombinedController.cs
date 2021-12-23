using ApplicationSmart.Combined;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSearchAPI.Controllers
{
    public class CombinedController : BaseController
    {
        [HttpPost]
        public async Task<ActionResult<SearchResultListVm>> Search(SearchInput request)
        {
            var vm = await Mediator.Send(request);
            return base.Ok(vm);
        }
    }
}
