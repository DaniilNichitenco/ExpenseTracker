using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Profiles
{
    public class ProfileForUpdateDto
    {
        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
