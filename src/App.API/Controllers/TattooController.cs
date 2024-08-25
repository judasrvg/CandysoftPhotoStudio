using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using App.Application.Abstractions;
using App.Domain.Entities;
using App.Domain.Enum;
using App.Application.Services.Command;
using App.Application.Services.Query;
using App.Application.DTOs;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TattooController : ControllerBase
    {
        private readonly ITattooCommandService _TattooCommandService;
        private readonly ITattooQueryService _TattooQueryService;

        public TattooController(ITattooCommandService TattooCommandService, ITattooQueryService TattooQueryService)
        {
            _TattooCommandService = TattooCommandService;
            _TattooQueryService = TattooQueryService;
        }

        ///TODO:Incluir un GetAllTattoo

        [HttpGet("{Id:long}/{configType}")]
        public async Task<IActionResult> Get(long Id)
        {
            var Tattoo = await _TattooQueryService.GetTattooAsync(Id);
            if (Tattoo == null)
                return NotFound();
            return Ok(Tattoo);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Get([FromBody] FormFilterGetTattoos filters)
        {
            var tattoos = await _TattooQueryService.GetTattoosAsync(filters);
            if (tattoos == null || !tattoos.Any())
                return NotFound();
            return Ok(tattoos);
        }

        [HttpPost("prioritized")]
        public async Task<IActionResult> prioritized([FromBody] FormFilterGetTattoos filters)
        {
            var tattoos = await _TattooQueryService.GetTattoosAsync(filters);
            if (tattoos == null || !tattoos.Any())
                return NotFound();
            return Ok(tattoos.OrderBy(x=>x.Order).Take(11));
        }

        [HttpPost("AddTattoo")]
        public async Task<IActionResult> AddTattoo([FromBody] TattooDto Tattoo)
        {
            var idResult=await _TattooCommandService.AddOrUpdateTattooAsync(Tattoo);
            Tattoo.Id = idResult;
            return CreatedAtAction(nameof(Get), new  { Id = idResult }, Tattoo);
        }

        [HttpPut("UpdateTattoo")]
        public async Task<IActionResult> UpdateTattoo([FromBody] TattooDto Tattoo)
        {
            await _TattooCommandService.AddOrUpdateTattooAsync(Tattoo);
            return CreatedAtAction(nameof(Get), new { Id = Tattoo.Id }, Tattoo);
        }

        [HttpDelete("{Id:long}")]
        public async Task<IActionResult> Delete(long Id)
        {
            await _TattooCommandService.DeleteTattooAsync(Id);
            return Ok(Id);
        }

        // Nuevo endpoint para incrementar el rating
        [HttpPatch("IncrementRating/{Id:long}")]
        public async Task<IActionResult> IncrementRating(long Id)
        {
            await _TattooCommandService.IncrementRatingAsync(Id);
            return Ok();
        }

        [HttpPut("ReorderTattoo")]
        public async Task<IActionResult> ReorderTattoo(TattooDto tattoo)
        {
            var updatedTattooList = await _TattooCommandService.ReorderTattooAsync(tattoo.Id, tattoo.Order);
            return Ok(updatedTattooList);
        }
    }
}
