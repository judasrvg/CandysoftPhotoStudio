using App.Domain.Entities;
using App.Domain.Enum;
using System.Threading.Tasks;
using App.Application.DTOs;

namespace App.Application.Abstractions
{

    public interface IConfigValueQueryService
    {
        Task<IEnumerable<ConfigValueDto>?> GetConfigValuesByTypeAsync( CacheValueType configType);
        Task<IEnumerable<ConfigValueDto>?> GetBasicsConfigValuesAsync();
    }

    public interface IConfigValueCommandService
    {
        Task<long> AddOrUpdateConfigValueAsync(ConfigValueDto ConfigValue);
        Task DeleteConfigValueAsync(long Id, CacheValueType configType);
    }

}
