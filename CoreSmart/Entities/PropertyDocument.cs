using CoreSmart.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSmart.Entities
{
    class PropertyDocument : IDocument
    {
        public int Id { get; set; }
        public string Index { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
    }
}
