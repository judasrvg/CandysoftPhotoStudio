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

namespace App.Application.Services.Query
{
    public class ConfigValueQueryService : IConfigValueQueryService
    {
        private readonly IReadRepository<ConfigValue> _repository;
        //private readonly RedisCacheManager _cacheManager;
        private readonly IMapper _mapper;
        private static readonly HashSet<CacheValueType> _basicValueTypes = new HashSet<CacheValueType> { CacheValueType.TikTokInstagram, CacheValueType.PhoneFacebook, CacheValueType.EmailAddress, CacheValueType.StudioLocation };

        private static readonly HashSet<CacheValueType> _offerValueTypes = new HashSet<CacheValueType> { CacheValueType.Offer15, CacheValueType.OfferPegnant, CacheValueType.OfferChild, CacheValueType.OfferWedding, CacheValueType.OfferCasual, CacheValueType.OfferIndividual };


        public ConfigValueQueryService(IReadRepository<ConfigValue> repository,/* RedisCacheManager cacheManager, */IMapper mapper)
        {
            _repository = repository;
            //_cacheManager = cacheManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ConfigValueDto>?> GetBasicsConfigValuesAsync()
        {
            //string cacheKey = KeyBuilder.BuildSimpleTypeBaseKey(CacheBaseKeyType.ConfigValue, configType);

            //// Intentar obtener los valores desde el caché
            //var cachedData = await _cacheManager.GetAsync<ConfigValueDto[]>(cacheKey);
            //if (cachedData != null)
            //{
            //    return cachedData;
            //}

            // Si no hay datos en el caché, obtenerlos desde la base de datos
            var configValues = await _repository.FindAsync(c=> _basicValueTypes.Contains( c.ValueType));
            if (configValues == null || !configValues.Any())
                return null;

            // Agrupar por tipo
            //var groupedConfigValues = configValues
            //    .GroupBy(cv => cv.ValueType)
            //    .ToDictionary(g => g.Key, g => _mapper.Map<IEnumerable<ConfigValueDto>>(g));

            //// Almacenar cada grupo en Redis
            //foreach (var kvp in groupedConfigValues)
            //{
            //    var groupCacheKey = KeyBuilder.BuildSimpleTypeBaseKey(CacheBaseKeyType.ConfigValue, kvp.Key); 
            //    await _cacheManager.SetAsync(groupCacheKey, kvp.Value);
            //}

            // Devolver el grupo solicitado
            return _mapper.Map<ConfigValueDto[]>(configValues);
        }

        public async Task<IEnumerable<ConfigValueDto>?> GetOfferConfigValuesAsync()
        {
            //string cacheKey = KeyBuilder.BuildSimpleTypeBaseKey(CacheBaseKeyType.ConfigValue, configType);

            //// Intentar obtener los valores desde el caché
            //var cachedData = await _cacheManager.GetAsync<ConfigValueDto[]>(cacheKey);
            //if (cachedData != null)
            //{
            //    return cachedData;
            //}

            // Si no hay datos en el caché, obtenerlos desde la base de datos
            var configValues = await _repository.FindAsync(c => _offerValueTypes.Contains(c.ValueType));
            if (configValues == null || !configValues.Any())
                return null;

            // Agrupar por tipo
            //var groupedConfigValues = configValues
            //    .GroupBy(cv => cv.ValueType)
            //    .ToDictionary(g => g.Key, g => _mapper.Map<IEnumerable<ConfigValueDto>>(g));

            //// Almacenar cada grupo en Redis
            //foreach (var kvp in groupedConfigValues)
            //{
            //    var groupCacheKey = KeyBuilder.BuildSimpleTypeBaseKey(CacheBaseKeyType.ConfigValue, kvp.Key); 
            //    await _cacheManager.SetAsync(groupCacheKey, kvp.Value);
            //}

            // Devolver el grupo solicitado
            return _mapper.Map<ConfigValueDto[]>(configValues);
        }

        public async Task<IEnumerable<ConfigValueDto>?> GetConfigValuesByTypeAsync(CacheValueType configType)
        {
            //string cacheKey = KeyBuilder.BuildSimpleTypeBaseKey(CacheBaseKeyType.ConfigValue, configType);

            //// Intentar obtener los valores desde el caché
            //var cachedData = await _cacheManager.GetAsync<ConfigValueDto[]>(cacheKey);
            //if (cachedData != null)
            //{
            //    return cachedData;
            //}

            // Si no hay datos en el caché, obtenerlos desde la base de datos
            var configValues = await _repository.GetAllAsync();
            if (configValues == null || !configValues.Any())
                return null;

            // Agrupar por tipo
            //var groupedConfigValues = configValues
            //    .GroupBy(cv => cv.ValueType)
            //    .ToDictionary(g => g.Key, g => _mapper.Map<IEnumerable<ConfigValueDto>>(g));

            //// Almacenar cada grupo en Redis
            //foreach (var kvp in groupedConfigValues)
            //{
            //    var groupCacheKey = KeyBuilder.BuildSimpleTypeBaseKey(CacheBaseKeyType.ConfigValue, kvp.Key); 
            //    await _cacheManager.SetAsync(groupCacheKey, kvp.Value);
            //}

            // Devolver el grupo solicitado
            return _mapper.Map<ConfigValueDto[]>(configValues.Where(x => x.ValueType == configType));
        }


    }
}

