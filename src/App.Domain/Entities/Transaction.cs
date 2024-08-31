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
        public long? ProductId { get; set; } // Nullable to allow transactions not related to a specific product
        public long? ReservationId { get; set; } // Nullable to allow transactions related to reservations
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public int Quantity { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal TotalAmount { get; set; }

        // Navigation properties
        public Product? Product { get; set; }
        public Reservation? Reservation { get; set; }
    }
}
