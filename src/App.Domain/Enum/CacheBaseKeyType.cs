using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Enum
{
    public enum CacheBaseKeyType
    {
        [Description("ConfigValue")]
        ConfigValue,
        [Description("Tattoo")]
        Tattoo,
    }
}
