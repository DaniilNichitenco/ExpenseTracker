using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Wallets
{
    public class WalletForListDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public double Bill { get; set; }
        public string CurrencyCode { get; set; }
    }
}
