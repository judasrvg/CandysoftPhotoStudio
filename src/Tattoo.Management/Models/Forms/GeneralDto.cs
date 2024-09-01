﻿using Tattoo.Management.Models.Forms.Enum;

namespace Tattoo.Management.Models.Forms
{

    public class TattooDto 
    {
        public long Id { get; set; }
        public string TattooName { get; set; } = string.Empty;
        public string TattooDescription { get; set; } = string.Empty;
        public long? TattooStyleId { get; set; }
        public long? TattooBodyPartId { get; set; }
        public long? TattooGenreId { get; set; }
        public ConfigValueDto? TattooStyle { get; set; }
        public ConfigValueDto? TattooBodyPart { get; set; }
        public ConfigValueDto? TattooGenre { get; set; }
        public int Rating { get; set; }
        public bool IsFavorite { get; set; }
        public int Order { get; set; }

        public string ImagePath { get; set; } = string.Empty;
        public string MiniatureImagePath { get; set; } = string.Empty;

        public TattooDto DeepClone()
        {
            return new TattooDto
            {
                Id = this.Id,
                TattooName = this.TattooName,
                TattooDescription = this.TattooDescription,
                //ConfigValues = this.ConfigValues.,
                ///TODO :Buscar como clonar listados
                ImagePath = this.ImagePath
            };
        }
    }

    public class ReservationDto
    {
        public long Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string ClientPhone { get; set; } = string.Empty;
        public int BudgetAmount { get; set; }
        public bool IsWithColor { get; set; }
        public bool IsCoverUp { get; set; }
        public DateTime ReservationDateStart { get; set; }
        public DateTime ReservationDateEnd { get; set; }
        public string[] ImagePaths { get; set; } = [];
        public AppoitmentStateType CurrentStateType { get; set; }
        public string Details { get; set; } = string.Empty;

        public string Lang { get; set; } = "es";
        public List<ConfigValueDto> Offers { get; set; } = new List<ConfigValueDto>();

        public decimal TotalAmount { get; set; }
        public bool Edited{ get; set; }
    }

    public class ConfigValueDto 
    {
        //public ConfigValueDto(CacheValueType cacheValue)
        //{
        //    ValueType = cacheValue;
        //}
        //public long Id { get; set; }
        //public CacheValueType ValueType { get; set; }
        //public string Value { get; set; } = string.Empty;
        //public string ValueDescription { get; set; } = string.Empty;
        
            public long Id { get; set; }
            public CacheValueType ValueType { get; set; }
            public string Value { get; set; } = string.Empty;
            public string ValueDescription { get; set; } = string.Empty;
            public bool IsSpecialValue { get; set; }
            public decimal PriceValue { get; set; }
        public ConfigValueDto DeepClone()
            {
                return new ConfigValueDto
                {
                    Id = this.Id,
                    ValueType = this.ValueType,
                    Value = this.Value,
                    ValueDescription = this.ValueDescription,
                    IsSpecialValue = this.IsSpecialValue,
                    PriceValue = this.PriceValue
                };
            }
        
    }

    public class FormFilterGetTattoos
    {
        public string TattooName { get; set; } = string.Empty;
        public long? TattooStyleId { get; set; }
        public long? TattooBodyPartId { get; set; }
        public long? TattooGenreId { get; set; }
    }

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
