using ApplicationSmart.Interfaces;
using AutoMapper;
using CoreSmart.Entities;
using System.Collections.Generic;
using System.Text;

namespace ApplicationSmart.Property.Queries.GetPropertiesList
{
    public class PropertiesListVm
    {
        public IList<PropertyLookUpDto> Properties { get; set; }
    }
}
