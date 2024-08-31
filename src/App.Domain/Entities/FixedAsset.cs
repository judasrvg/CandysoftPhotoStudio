using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class FixedAsset
    {
        public long ProductId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiryDate { get; set; }
        public decimal? DepreciationRate { get; set; }

        // Navigation property
        public Product Product { get; set; } = null!;
    }

}
