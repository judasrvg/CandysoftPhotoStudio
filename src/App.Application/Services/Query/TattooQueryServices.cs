using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.Abstractions;
using App.Application.Cache;
using App.Application.DTOs;
using App.Domain.Entities;
using App.Domain.Enum;
using App.Domain.Interfases;
using Newtonsoft.Json;
using App.Application.DTOs;
using System.Linq.Expressions;
using App.Domain.ValueObjects;

namespace App.Application.Services.Query
{
    public class TattooQueryService : ITattooQueryService
    {
        private readonly IReadRepository<Tattoo> _repository;
        //private readonly RedisCacheManager _cacheManager;
        private readonly IMapper _mapper;

        public TattooQueryService(IReadRepository<Tattoo> repository, /*RedisCacheManager cacheManager,*/ IMapper mapper)
        {
            _repository = repository;
            //_cacheManager = cacheManager;
            _mapper = mapper;
        }

        public async Task<TattooDto?> GetTattooAsync(long id)
        {
            //string cacheKey = KeyBuilder.BuildCacheKey(existKey: true, CacheBaseKeyType.Tattoo, id, CacheValueType.Tattoo);
            //var cachedData = await _cacheManager.GetAsync<TattooDto>(cacheKey);
            //if (cachedData != null)
            //    return cachedData;

            var agentConfigs = await _repository.GetByIdAsync(id);
            if (agentConfigs == null )
                return null;

            var configsToTransfer = _mapper.Map<TattooDto>(agentConfigs);

            // Guardar todas las configuraciones en un hash
            //await _cacheManager.SetMultipleAsync(KeyBuilder.BuildCacheKey(existKey: false, CacheBaseKeyType.Tattoo, id, CacheValueType.Tattoo), configsToTransfer, a => CacheValueType.Tattoo.ToString());

            return configsToTransfer;
        }

        public async Task<IEnumerable<TattooDto>> GetTattoosAsync(FormFilterGetTattoos filters)
        {
            
            //string cacheKey = KeyBuilder.BuildSimpleTypeBaseKey(CacheBaseKeyType.Tattoo, CacheValueType.Tattoo);
            //var cachedData = await _cacheManager.GetAsync<IEnumerable<TattooDto>>(cacheKey);
            //if (cachedData == null || !cachedData.Any())
            //{
                var queryByStyle =  await _repository.GetAllAsync();

                var cachedData = _mapper.Map<IEnumerable<TattooDto>>(queryByStyle);
            //    await _cacheManager.SetAsync(cacheKey, cachedData);

            //}

            if (!string.IsNullOrEmpty(filters.TattooName))
            {
                cachedData = cachedData.Where(t => t.TattooName.Contains(filters.TattooName));
            }
            if (filters.TattooStyleId.HasValue)
            {
                cachedData = cachedData.Where(t => t.TattooStyleId == filters.TattooStyleId);
            }
            if (filters.TattooBodyPartId.HasValue)
            {
                cachedData = cachedData.Where(t => t.TattooBodyPartId == filters.TattooBodyPartId);
            }
            if (filters.TattooGenreId.HasValue)
            {
                cachedData = cachedData.Where(t => t.TattooGenreId == filters.TattooGenreId);
            }
            return cachedData;
        }

        ///This version Use tattoostyleId for store caching not used in this case for future
        //public async Task<IEnumerable<TattooDto>> GetTattoosAsync(FormFilterGetTattoos filters)
        //{
        //    if (filters.TattooStyleId.HasValue && filters.TattooStyleId != 0)
        //    {

        //    }
        //    string cacheKey = KeyBuilder.BuildCacheKey(existKey: true, CacheBaseKeyType.Tattoo, filters.TattooStyleId??0, CacheValueType.TattooStyle);
        //    var cachedData = await _cacheManager.GetAsync<IEnumerable<TattooDto>>(cacheKey);
        //    if (cachedData == null || !cachedData.Any())
        //    {
        //        var queryByStyle = filters.TattooStyleId.HasValue ? await _repository.FindAsync(t => t.TattooStyleId == filters.TattooStyleId): await _repository.GetAllAsync();

        //        cachedData = _mapper.Map<IEnumerable<TattooDto>>(queryByStyle);
        //        await _cacheManager.SetAsync(cacheKey, cachedData);

        //    }

        //    if (!string.IsNullOrEmpty(filters.TattooName))
        //    {
        //        cachedData = cachedData.Where(t => t.TattooName.Contains(filters.TattooName));
        //    }
        //    if (filters.TattooStyleId.HasValue)
        //    {
        //        cachedData = cachedData.Where(t => t.TattooStyleId == filters.TattooStyleId);
        //    }
        //    if (filters.TattooBodyPartId.HasValue)
        //    {
        //        cachedData = cachedData.Where(t => t.TattooBodyPartId == filters.TattooBodyPartId);
        //    }
        //    if (filters.TattooGenreId.HasValue)
        //    {
        //        cachedData = cachedData.Where(t => t.TattooGenreId == filters.TattooGenreId);
        //    }
        //    return cachedData;
        //}

        /// <summary>
        /// NOT Already used but take in count for future predicate builder
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        private Expression<Func<Tattoo, bool>> BuildPredicate(FormFilterGetTattoos filters)
        {
            var predicate = PredicateBuilder.True<Tattoo>();

            if (!string.IsNullOrEmpty(filters.TattooName))
            {
                predicate = predicate.And(t => t.TattooName.Contains(filters.TattooName));
            }
            if (filters.TattooStyleId.HasValue)
            {
                predicate = predicate.And(t => t.TattooStyleId == filters.TattooStyleId);
            }
            if (filters.TattooBodyPartId.HasValue)
            {
                predicate = predicate.And(t => t.TattooBodyPartId == filters.TattooBodyPartId);
            }
            if (filters.TattooGenreId.HasValue)
            {
                predicate = predicate.And(t => t.TattooGenreId == filters.TattooGenreId);
            }

            return predicate;
        }


        ///TODO:Use This for multiple tattoo insertion on Redis
        //public async Task<IEnumerable<ConfigValueDto>?> GetConfigValuesByTypeAsync(CacheValueType configType)
        //{
        //    string cacheKey = KeyBuilder.BuildCacheKey(existKey: true, CacheBaseKeyType.ConfigValue, id: 0, configType);

        //    // Intentar obtener los valores desde el caché
        //    var cachedData = await _cacheManager.GetAsync<ConfigValue>(cacheKey);
        //    if (cachedData != null && cachedData.Any())
        //    {
        //        var cachedValues = cachedData.Values.Select(value =>
        //        {
        //            var jsonString = Encoding.UTF8.GetString(value);
        //            return JsonConvert.DeserializeObject<ConfigValueDto>(jsonString);
        //        });
        //        return cachedValues;
        //    }

        //    // Si no hay datos en el caché, obtenerlos desde la base de datos
        //    var configValues = await _repository.FindAsync(ac => ac.ValueType == configType);
        //    if (configValues == null || !configValues.Any())
        //        return null;

        //    var configValuesToTransfer = _mapper.Map<ConfigValueDto[]>(configValues);

        //    // Guardar todas las configuraciones en el caché
        //    await _cacheManager.SetMultipleByKeyAsync(configValuesToTransfer.Select(x => (x.Id, x)).ToArray(), CacheBaseKeyType.ConfigValue, configType);

        //    return configValuesToTransfer;
        //}
    }
}
