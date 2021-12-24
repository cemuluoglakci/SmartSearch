using MediatR;
using System.Collections.Generic;

namespace ApplicationSmart.CombinedSearch.FilteredSearch
{
    public class FilteredSearchInput : IRequest<FilteredSearchResultListVm>
    {
        // Usually I use auto property initializer:
        // public int Limit { get; set; } = 25;
        // However, because of C#5, even it does not feel right, I will assign default value in constructor:
        public FilteredSearchInput()
        {
            Limit = 25;
        }

        public string SearchPhrase { get; set; }
        public List<string> MarketList { get; set; }
        public List<string> StateList { get; set; }
        public int Limit { get; set; }
    }
}
