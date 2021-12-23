using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationSmart.Combined
{
    public class SearchInput : IRequest<SearchResultListVm>
    {
        // Usually I use auto property initializer:
        // public int Limit { get; set; } = 25;
        // However, because of C#5, even it does not feel right, I will assign default value in constructor:
        public SearchInput() 
        {
            Limit = 25;
        }

        public string SearchPhrase { get; set; }
        public List<string> MarketList { get; set; }
        public List<string> StateList { get; set; }
        public int Limit { get; set; }
    }
}
