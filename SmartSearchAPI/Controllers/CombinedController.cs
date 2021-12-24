using ApplicationSmart.CombinedSearch;
using ApplicationSmart.CombinedSearch.BasicSearch;
using ApplicationSmart.CombinedSearch.FilteredSearch;
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
        public async Task<ActionResult<SearchResultListVm>> BasicSearch(SearchInput request)
        {
            var vm = await Mediator.Send(request);
            return base.Ok(vm);
        }

        [HttpPost]
        public async Task<ActionResult<FilteredSearchResultListVm>> FilteredSearch(FilteredSearchInput request)
        {
            var vm = await Mediator.Send(request);
            return base.Ok(vm);
        }

    }
}
