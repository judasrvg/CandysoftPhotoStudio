using App.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProductType ProductType { get; set; }
        public TransactionGroup TransactionGroup { get; set; }
        public decimal Price { get; set; }
        public decimal PurchaseCost { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public int StockQuantity { get; set; } = 0; // Inicializado en 0
        public int TotalQuantity { get; set; } = 0; // Inicializado en 0
        public string ImagePath { get; set; } = string.Empty;
        public string MiniatureImagePath { get; set; } = string.Empty;

        // Navigation properties
        public FixedAsset? FixedAsset { get; set; }
        public Merchandise? Merchandise { get; set; }
        public RawMaterial? RawMaterial { get; set; }


    }

}
