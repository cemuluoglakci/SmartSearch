using System;
using System.Collections.Generic;
using System.Text;

namespace CoreSmart.Interfaces
{
    public interface IDocument
    {
        int Id { get; set; }
        string Index { get; set; }
        string Name { get; set; }
        string Market { get; set; }
        string State { get; set; }
    }
}
