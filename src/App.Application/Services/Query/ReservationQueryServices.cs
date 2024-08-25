using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Application.Abstractions;
using App.Application.Cache;
using App.Application.DTOs;
using App.Domain.Entities;
using App.Domain.Enum;
using App.Domain.Interfases;

namespace App.Application.Services.Query
{
    public class ReservationQueryService : IReservationQueryService
    {
        private readonly IReadRepository<Reservation> _repository;
        //private readonly RedisCacheManager _cacheManager;
        private readonly IMapper _mapper;

        public ReservationQueryService(IReadRepository<Reservation> repository, /*RedisCacheManager cacheManager,*/ IMapper mapper)
        {
            _repository = repository;
            //_cacheManager = cacheManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReservationDto>?> GetAllReservationsAsync()
        {
            var appointments = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReservationDto>>(appointments);
        }

        public Task<int> GetCountSolicitedReservationsAsync()
        {
            return _repository.FindAndCountAsync(x => x.CurrentStateType == AppoitmentStateType.Solicited);
        }

        public async Task<ReservationDto?> GetReservationAsync(long id)
        {
            return null;
        }

        public async Task<IEnumerable<ReservationDto>> GetUpcomingReservationsAsync(TimeSpan timeBeforeStart)
        {
            var now = DateTime.Now;
            var notificationTime = now.Add(timeBeforeStart);

            var reservations = await _repository.FindAsync(r =>
                r.ReservationDateStart >= now && r.ReservationDateStart <= notificationTime.AddHours(3) && !r.Notified && r.CurrentStateType==AppoitmentStateType.Confirmed);

            return _mapper.Map<IEnumerable<ReservationDto>>(reservations);
        }

    }
}
