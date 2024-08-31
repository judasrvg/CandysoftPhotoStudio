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
using App.Domain.State;
using App.Application.Abstractions.Services;
using App.Application.DTOs;

namespace App.Application.Services.Command
{
    public class ReservationCommandService : IReservationCommandService
    {
        private readonly IReadRepository<Reservation> _readRepository;
        private readonly IWriteRepository<Reservation> _writeRepository;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly RedisCacheManager _cacheManager;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;

        public ReservationCommandService(IReadRepository<Reservation> readRepository, IWriteRepository<Reservation> writeRepository, IUnitOfWork unitOfWork, /*RedisCacheManager cacheManager, */IMapper mapper, IEventPublisher eventPublisher)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _unitOfWork = unitOfWork;
            //_cacheManager = cacheManager;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }

        public async Task<long> AddOrUpdateReservationAsync(ReservationDto reservation)
        {
            bool existKey = true;
            var existingConfig = new Reservation();
            //var result = new Reservation();
            if (reservation.Id == 0)
            {
                existKey = false;
            }
            if (existKey)
            {  
                existingConfig = await _readRepository.GetByIdAsync(reservation.Id);
                existKey = existingConfig != null;
            
            }

            if (existKey)
            {
                //existingConfig = _mapper.Map<Reservation>(reservation);
                existingConfig.ClientName = reservation.ClientName;
                existingConfig.ClientEmail = reservation.ClientEmail;
                existingConfig.ClientPhone = reservation.ClientPhone;
                existingConfig.Details = reservation.Details;
                //existingConfig.ImagePath = reservation.ImagePath;
                existingConfig.ImagePathsJson = JsonConvert.SerializeObject(reservation.ImagePaths);
                existingConfig.Offers = reservation.Offers.Select(x=>new ConfigValue 
                {
                    Id=x.Id,
                    IsSpecialValue=x.IsSpecialValue,
                    PriceValue=x.PriceValue ,
                    Value=x.Value,
                    ValueDescription=x.ValueDescription,
                    ValueType=x.ValueType
                }).ToList();
                //existingConfig.TotalAmount = reservation.TotalAmount;
                existingConfig.ReservationDateEnd = reservation.ReservationDateEnd;
                existingConfig.ReservationDateStart = reservation.ReservationDateStart;
                existingConfig.Lang = reservation.Lang;
                existingConfig.IsCoverUp = reservation.IsCoverUp;
                existingConfig.IsWithColor = reservation.IsWithColor;
                existingConfig.Notified = reservation.Notified;
                if (reservation.CurrentStateType != existingConfig.CurrentStateType)
                {
                    if (reservation.CurrentStateType == AppoitmentStateType.Canceled)
                    {
                        existingConfig!.Cancel();

                    }
                    else if (reservation.CurrentStateType == AppoitmentStateType.Confirmed)
                    {
                        existingConfig!.JumpToState(new ConfirmedState());
                    }
                    else
                    {
                        existingConfig!.JumpToState(new SolicitedState());
                    }

                }
                _writeRepository.Update(existingConfig);
            }
            else
            {
                existingConfig = await _writeRepository.AddAsync(_mapper.Map<Reservation>(reservation));
            }

            await _unitOfWork.CompleteAsync();

            // Publish event
            if (!existKey)
            {
                await _eventPublisher.Publish(new AppointmentCreatedEvent(reservation));

            }
            return existingConfig?.Id ?? 0;

        }

        public async Task DeleteReservationAsync(long id)
        {
            //var Reservation = await _readRepository.GetByIdAsync(id, configType);
            //if (Reservation != null)
            //{
            //    _writeRepository.Delete(Reservation);
            //    await _unitOfWork.CompleteAsync();
            //    string cacheKey = KeyBuilder.BuildCacheKey(existKey: true, CacheBaseKeyType.Reservation, id, configType);
            //    await _cacheManager.RemoveAsync(cacheKey);
            //}
        }
    }
}
