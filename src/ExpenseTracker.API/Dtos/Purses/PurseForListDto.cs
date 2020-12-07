using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Purses
{
    public class PurseForListDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public double Bill { get; set; }
        public string CurrencyCode { get; set; }
    }
}
