using App.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class Transaction
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public int Quantity { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal TotalAmount { get; set; }

        // Navigation property
        public Product Product { get; set; } = null!;
    }
}
