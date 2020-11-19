using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Purses
{
    public class PurseForCreateDto
    {
        [Required]
        [Range(0.0d, Double.MaxValue)]
        public double Bill { get; set; }

        [Required]
        [MaxLength(3)]
        public string CurrencyCode { get; set; }
    }
}
