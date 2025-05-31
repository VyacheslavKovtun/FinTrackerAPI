using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrackerAPI.Domain.Core.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public int CurrencyId { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
    }
}
