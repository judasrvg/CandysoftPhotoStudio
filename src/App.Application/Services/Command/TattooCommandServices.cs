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
    public class TattooCommandService : ITattooCommandService
    {
        private readonly IReadRepository<Tattoo> _readRepository;
        private readonly IWriteRepository<Tattoo> _writeRepository;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly RedisCacheManager _cacheManager;
        private readonly IMapper _mapper;

        public TattooCommandService(IReadRepository<Tattoo> readRepository, IWriteRepository<Tattoo> writeRepository, IUnitOfWork unitOfWork, /*RedisCacheManager cacheManager,*/ IMapper mapper)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _unitOfWork = unitOfWork;
            //_cacheManager = cacheManager;
            _mapper = mapper;
        }

        /// <summary>
        /// All Tattoos are stored in caching under one key
        /// </summary>
        /// <param name="tattoo"></param>
        /// <returns></returns>
        public async Task<long> AddOrUpdateTattooAsync(TattooDto tattoo)
        {
            var existTattoo = await _readRepository.GetByIdAsync(tattoo.Id);
            bool existKey = existTattoo != null;
            var result = new Tattoo();
            if (existKey)
            {
                existTattoo!.TattooName = tattoo.TattooName;
                existTattoo!.TattooDescription = tattoo.TattooDescription;
                existTattoo!.TattooGenreId = tattoo.TattooGenreId;
                existTattoo!.TattooBodyPartId = tattoo.TattooBodyPartId;
                existTattoo!.TattooStyleId = tattoo.TattooStyleId;
                existTattoo!.ImagePath = tattoo.ImagePath;
                existTattoo!.MiniatureImagePath = tattoo.MiniatureImagePath;
                existTattoo!.Rating = tattoo.Rating;
                existTattoo!.Order = tattoo.Order;
                _writeRepository.Update(existTattoo);
            }
            else
            {
               result= await _writeRepository.AddAsync(_mapper.Map<Tattoo>(tattoo));
            }

            await _unitOfWork.CompleteAsync();
            return result?.Id ?? 0;

        }

        public async Task DeleteTattooAsync(long id)
        {
            
             var tattoo = await _readRepository.GetByIdAsync(id);
             if (tattoo != null)
             {
                 _writeRepository.Delete(tattoo);
                 await _unitOfWork.CompleteAsync();

            }

        }

        public async Task IncrementRatingAsync(long tattooId)
        {
            var tattoo = await _readRepository.GetByIdAsync(tattooId);
            if (tattoo == null)
                throw new KeyNotFoundException("Tattoo not found");

            tattoo.IncrementRating();
            _writeRepository.Update(tattoo);
            await _unitOfWork.CompleteAsync();

        }

        public async Task<List<TattooDto>> ReorderTattooAsync(long tattooId, int newOrder)
        {
            var tattoos = await _readRepository.GetAllAsync();
            var tattooToUpdate = tattoos.FirstOrDefault(t => t.Id == tattooId);
            if (tattooToUpdate == null) throw new KeyNotFoundException("Tattoo not found");

            // Ordenar tatuajes por su orden actual
            var orderedTattoos = tattoos.OrderBy(t => t.Order).ToList();
            var currentOrder = tattooToUpdate.Order;

            if (currentOrder == newOrder) return _mapper.Map<List<TattooDto>>(orderedTattoos);

            // Quitar el tatuaje que se va a mover de la lista ordenada
            orderedTattoos.Remove(tattooToUpdate);

            // Insertar el tatuaje en la nueva posición
            if (newOrder <= orderedTattoos.Count)
            {
                orderedTattoos.Insert(newOrder - 1, tattooToUpdate); // Ajustar para índice basado en 1
            }
            else
            {
                orderedTattoos.Add(tattooToUpdate);
            }

            // Recalcular los órdenes
            for (int i = 0; i < orderedTattoos.Count; i++)
            {
                orderedTattoos[i].Order = i + 1; // Ajustar para órdenes basados en 1
            }

            // Actualizar los tatuajes en la base de datos
            await _writeRepository.BulkUpdateAsync(orderedTattoos);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<List<TattooDto>>(orderedTattoos);
        }



    }
}
