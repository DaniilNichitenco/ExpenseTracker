using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Infrastructure.Models
{
    public class RequestFilters
    {
        public RequestFilters()
        {

        }

        public FilterLogicalOperators LogicalOperators { get; set; }
        public IList<Filter> Filters { get; set; }
    }
}
