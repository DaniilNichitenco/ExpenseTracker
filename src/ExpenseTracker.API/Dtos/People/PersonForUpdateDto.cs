using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.People
{
    public class PersonForUpdateDto
    {
        [Required]
        public int OwnerId { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Email { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string UserName { get; set; }
    }
}
