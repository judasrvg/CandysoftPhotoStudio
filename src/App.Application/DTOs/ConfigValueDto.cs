using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.Abstractions.Implementations;
using App.Domain.Enum;

namespace App.Application.DTOs
{
    public class ConfigValueDto:IDeepCloneable<ConfigValueDto>
    {
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
}
