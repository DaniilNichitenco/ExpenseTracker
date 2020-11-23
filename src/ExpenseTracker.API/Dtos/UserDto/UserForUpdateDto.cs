using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.UserDto
{
    public class UserForUpdateDto
    {
        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string LastName { get; set; }
    }
}
