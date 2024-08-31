using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class Merchandise
    {
        public long ProductId { get; set; }
        public int StockQuantity { get; set; }
        public DateTime? LastRestockedDate { get; set; }

        // Navigation property
        public Product Product { get; set; } = null!;
    }

}
