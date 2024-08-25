using Tattoo.StudioUI.Models.Forms.Enum;

namespace Tattoo.StudioUI.Models.Forms
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

        public string ImagePath { get; set; } = string.Empty;

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
        public string ImagePath { get; set; } = string.Empty;
        public AppoitmentStateType CurrentStateType { get; set; }
        public string Details { get; set; } = string.Empty;

        
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

            public ConfigValueDto DeepClone()
            {
                return new ConfigValueDto
                {
                    Id = this.Id,
                    ValueType = this.ValueType,
                    Value = this.Value,
                    ValueDescription = this.ValueDescription
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
