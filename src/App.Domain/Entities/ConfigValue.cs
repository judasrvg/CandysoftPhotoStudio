using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Enum;

namespace App.Domain.Entities
{
    //This class is only for simple object with id-value-description
    public class ConfigValue
    {
        [Key]
        [Required]
        public long Id { get; set; }
        public CacheValueType ValueType { get; set; } 
        public string Value { get; set; } = string.Empty;
        public string ValueDescription { get; set; } = string.Empty;

    }

}
