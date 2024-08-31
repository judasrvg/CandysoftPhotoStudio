using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class RawMaterial
    {
        public long ProductId { get; set; }
        public int StockQuantity { get; set; }
        public decimal? ConsumptionRate { get; set; }
        public DateTime? LastUsedDate { get; set; }

        // Navigation property
        public Product Product { get; set; } = null!;
    }

}
