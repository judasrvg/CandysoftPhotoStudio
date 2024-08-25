using App.Domain.Entities;
using App.Domain.Enum;
using System.Threading.Tasks;
using App.Application.DTOs;

namespace App.Application.Abstractions
{
    
    public interface IReservationQueryService
    {
        Task<int> GetCountSolicitedReservationsAsync();
        Task<IEnumerable<ReservationDto>?> GetAllReservationsAsync();
        Task<ReservationDto?> GetReservationAsync(long id);
        Task<IEnumerable<ReservationDto>> GetUpcomingReservationsAsync(TimeSpan timeBeforeStart);
    }

    public interface IReservationCommandService
    {
        Task AddOrUpdateReservationAsync(ReservationDto Reservation);
        Task DeleteReservationAsync(long id);
    }

}
