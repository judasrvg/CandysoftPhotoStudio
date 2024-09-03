using App.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs
{
    public class StockRequest
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public bool WithTransaction{ get; set; }
    }

    public class StockDecrement
    {
        public StockRequest[] Requests { get; set; } = [];
    }

    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProductType ProductType { get; set; }
        public decimal Price { get; set; }
        public decimal PurchaseCost { get; set; }
        public DateTime CreatedDate { get; set; }= DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public int StockQuantity { get; set; } = 0; // Inicializado en 0
        public int TotalQuantity { get; set; } = 0; // Inicializado en 0
        public string ImagePath { get; set; } = string.Empty;
        public string MiniatureImagePath { get; set; } = string.Empty;

        public FixedAssetDto? FixedAssetDto { get; set; }
        public MerchandiseDto? MerchandiseDto { get; set; }
        //public RawMaterialDto? RawMaterialDto { get; set; }
    }

    public class FixedAssetDto
    {
        public long ProductId { get; set; }
        public DateTime? PurchaseDate { get; set; } = DateTime.Now;
        public DateTime? WarrantyExpiryDate { get; set; } = DateTime.Now;
        public decimal? DepreciationRate { get; set; }
        //public int Quantity { get; set; }
    }
    public class MerchandiseDto
    {
        public long ProductId { get; set; }
        public DateTime? LastRestockedDate { get; set; } = DateTime.Now;
    }
    public class RawMaterialDto
    {
        public long ProductId { get; set; }
        //public int StockQuantity { get; set; }
        public decimal? ConsumptionRate { get; set; }
        public DateTime? LastUsedDate { get; set; } = DateTime.Now;
    }
    public class TransactionDto
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public long? ReservationId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public int Quantity { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal TotalAmount { get; set; }
        public string Description { get; set; } = string.Empty;

    }

}
