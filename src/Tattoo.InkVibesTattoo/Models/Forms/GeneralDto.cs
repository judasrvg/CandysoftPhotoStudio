﻿using Tattoo.InkVibesTattoo.Models.Forms.Enum;

namespace Tattoo.InkVibesTattoo.Models.Forms
{
    public class Offer
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<string> Features { get; set; }
        //public string Icon { get; set; } // Para el ícono de Material Design
        public bool IsHighlighted { get; set; } // Para destacar la tarjeta "Estándar"
    }

    public class LanguageOption
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

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
        public bool IsFavorite { get; set; }
        public int Rating { get; set; }
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
        public string Lang { get; set; } = "en";
        public List<ConfigValueDto> Offers { get; set; } = new List<ConfigValueDto>();

        public decimal TotalAmount { get; set; }
        public bool Edited { get; set; }

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
                    PriceValue = this.PriceValue,
                     IsSpecialValue = this.IsSpecialValue
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
}
