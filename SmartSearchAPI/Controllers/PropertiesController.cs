using ApplicationSmart.Property.Queries.GetPropertiesList;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSearchAPI.Controllers
{
    //Add authorization and authentication
    public class PropertiesController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<PropertiesListVm>> GetAll()
        {
            var vm = await Mediator.Send(new GetPropertiesListQuery());
            return base.Ok(vm);
        }
    }
}
