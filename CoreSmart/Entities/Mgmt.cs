using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreSmart.Entities
{
    public class Mgmt
    {
        [Key]
        public int MgmtID { get; set; }
        public string Name { get; set; }
        public string Market { get; set; }
        public string State { get; set; }
    }
}
