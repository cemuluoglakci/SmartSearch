using CoreSmart.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSmart.Entities
{
    public class MgmtDocument : IDocument
    {
        public int Id { get; set; }
        public string Index { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
        public string FormerName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
    }
}
