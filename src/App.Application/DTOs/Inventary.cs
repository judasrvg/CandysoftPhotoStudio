using App.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProductType ProductType { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Propiedades específicas para cada tipo
        public FixedAssetDto? FixedAssetDto { get; set; }
        public MerchandiseDto? MerchandiseDto { get; set; }
        public RawMaterialDto? RawMaterialDto { get; set; }
    }

    public class FixedAssetDto
    {
        public long ProductId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiryDate { get; set; }
        public decimal? DepreciationRate { get; set; }
    }
    public class MerchandiseDto
    {
        public long ProductId { get; set; }
        public int StockQuantity { get; set; }
        public DateTime? LastRestockedDate { get; set; }
    }
    public class RawMaterialDto
    {
        public long ProductId { get; set; }
        public int StockQuantity { get; set; }
        public decimal? ConsumptionRate { get; set; }
        public DateTime? LastUsedDate { get; set; }
    }
    public class TransactionDto
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public long? ReservationId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Quantity { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal TotalAmount { get; set; }
    }

}
