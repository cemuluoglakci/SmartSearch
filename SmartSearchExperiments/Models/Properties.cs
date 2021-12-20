﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSearchExperiments.Models
{
    public class Properties
    {
        public int PropertyID { get; set; }
        public string Name { get; set; }
        public string FormerName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

    }
}
