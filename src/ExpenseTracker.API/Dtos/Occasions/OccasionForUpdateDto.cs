using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Occasions
{
    public class OccasionForUpdateDto
    {
        [Required]
        public DateTime OccasionDate { get; set; }
        
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(int.MaxValue, MinimumLength = 0)]
        public string Context { get; set; }
    }
}
