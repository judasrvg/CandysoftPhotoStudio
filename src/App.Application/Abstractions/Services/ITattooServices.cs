using App.Domain.Entities;
using App.Domain.Enum;
using System.Threading.Tasks;
using App.Application.DTOs;

namespace App.Application.Abstractions
{
    
    public interface ITattooQueryService
    {
        Task<TattooDto?> GetTattooAsync(long Id);
        Task<IEnumerable<TattooDto>> GetTattoosAsync(FormFilterGetTattoos filters);
    }

    public interface ITattooCommandService
    {
        Task<long> AddOrUpdateTattooAsync(TattooDto agentConfig);
        Task DeleteTattooAsync(long id);
        Task IncrementRatingAsync(long tattooId);
        Task<List<TattooDto>> ReorderTattooAsync(long tattooId, int newOrder);
    }

}
