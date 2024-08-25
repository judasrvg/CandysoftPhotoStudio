using AutoMapper;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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

namespace App.Application.Services.Command
{
    public class ConfigValueCommandService : IConfigValueCommandService
    {
        private readonly IReadRepository<ConfigValue> _readRepository;
        private readonly IWriteRepository<ConfigValue> _writeRepository;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly RedisCacheManager _cacheManager;
        private readonly IMapper _mapper;

        public ConfigValueCommandService(IReadRepository<ConfigValue> readRepository, IWriteRepository<ConfigValue> writeRepository, IUnitOfWork unitOfWork, /*RedisCacheManager cacheManager, */IMapper mapper)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _unitOfWork = unitOfWork;
            //_cacheManager = cacheManager;
            _mapper = mapper;
        }

        public async Task<long> AddOrUpdateConfigValueAsync(ConfigValueDto configValue)
        {
            var existingConfig = await _readRepository.GetByIdAsync(configValue.Id);
            ConfigValue inserted = null;
            bool existKey = existingConfig != null;

            if (existKey)
            {
                if (existingConfig!.Value == configValue!.Value && existingConfig!.ValueDescription == configValue.ValueDescription)
                {
                    throw new InvalidOperationException("The value you entered already exists");
                }
                existingConfig!.Value = configValue.Value;
                existingConfig!.ValueDescription = configValue.ValueDescription;
                _writeRepository.Update(existingConfig);
            }
            else
            {
                inserted = await _writeRepository.AddAsync(_mapper.Map<ConfigValue>(configValue));
                return inserted.Id;
            }

            await _unitOfWork.CompleteAsync();

            // Actualizar el ID en caso de inserción
            //if (!existKey)
            //{
            //    configValue.Id = inserted.Id;
            //}

            //// Obtener todas las configuraciones desde la base de datos
            //var configValues = await _readRepository.FindAsync(x=>x.ValueType == configValue.ValueType);

            //// Almacenar cada grupo en Redis

            //var groupCacheKey = KeyBuilder.BuildSimpleTypeBaseKey(CacheBaseKeyType.ConfigValue, configValue.ValueType);
            //await _cacheManager.SetAsync(groupCacheKey,configValues);

            return configValue.Id;

        }

        public async Task DeleteConfigValueAsync(long id, CacheValueType configType)
        {
            var ConfigValue = await _readRepository.GetByIdAsync(id);
            if (ConfigValue != null)
            {
                _writeRepository.Delete(ConfigValue);
                await _unitOfWork.CompleteAsync();

                //var configValues = await _readRepository.FindAsync(x => x.ValueType == configType);
                //var groupCacheKey = KeyBuilder.BuildSimpleTypeBaseKey(CacheBaseKeyType.ConfigValue, configType);

                //// Almacenar cada grupo en Redis
                //if (configValues == null || !configValues.Any())
                //{
                //    await _cacheManager.RemoveAsync(groupCacheKey);
                //    return; 
                //}
                //await _cacheManager.SetAsync(groupCacheKey, configValues);

            }
        }
    }
}
