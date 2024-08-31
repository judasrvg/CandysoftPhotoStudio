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

            var agentConfigs = await _repository.GetByIdAsync(id);
            if (agentConfigs == null )
                return null;

            var configsToTransfer = _mapper.Map<TattooDto>(agentConfigs);

            return configsToTransfer;
        }

        public async Task<IEnumerable<TattooDto>> GetTattoosAsync(FormFilterGetTattoos filters)
        {
            
                var queryByStyle =  await _repository.GetAllAsync();

                var cachedData = _mapper.Map<IEnumerable<TattooDto>>(queryByStyle);

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

    }
}
