using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Enum;

namespace App.Application.Cache
{
    public static class KeyBuilder
    {
        public static string BuildCacheKey(bool existKey, CacheBaseKeyType keyType, long id, CacheValueType configType)
        {
            return BuildComposedConfigKey(keyType, id,configType);
            //return existKey ? BuildExistentConfigKey(keyType, id, configType) : BuildNewConfigBaseKey(keyType, id);
        }

        public static string BuildSimpleTypeBaseKey(CacheBaseKeyType keyType, CacheValueType configType)
        {
            string baseKey = keyType.GetDescription();
            string type = configType.GetDescription();
            return $"{baseKey}:{type}";
        }

        private static string BuildNewConfigBaseKey(CacheBaseKeyType keyType, long id)
        {
            string baseKey = keyType.GetDescription();
            return $"{baseKey}:{id}";
        }

        private static string BuildComposedConfigKey(CacheBaseKeyType keyType, long id, CacheValueType configType)
        {
            string baseKey = keyType.GetDescription();
            string valueType = configType.GetDescription();
            return baseKey == valueType ? $"{baseKey}:{id}" : $"{baseKey}:{valueType}:{id}";
        }


    }

}
