using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.API.Dtos.Expenses
{
    public class ExpenseForUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int WalletId { get; set; }

        [Required]
        [MaxLength(40)]
        public string Title { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [Range(0.0d, Double.MaxValue)]
        public double Money { get; set; }

        [Required]
        public int TopicId { get; set; }
    }
}
