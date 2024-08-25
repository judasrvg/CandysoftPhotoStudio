using App.Application.Abstractions;
using App.Application.Abstractions.Services;
using App.Application.DTOs;
using App.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationCommandService _ReservationCommandService;
        private readonly IReservationQueryService _ReservationQueryService;
        private readonly IEmailService _emailService;
        public ReservationController(IReservationCommandService ReservationCommandService, IReservationQueryService ReservationQueryService, IEmailService emailService)
        {
            _ReservationCommandService = ReservationCommandService;
            _ReservationQueryService = ReservationQueryService;
            _emailService = emailService;
        }

        ///TODO:Incluir un GetAllReservation
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Reservation = await _ReservationQueryService.GetAllReservationsAsync();
            if (Reservation == null)
                return NotFound();
            return Ok(Reservation);
        }

        [HttpGet("GetTotalSolicitedReservation")]
        public async Task<IActionResult> GetTotalSolicitedReservation()
        {
            var solicitedReservationsCount = await _ReservationQueryService.GetCountSolicitedReservationsAsync();
            return Ok(solicitedReservationsCount);
        }


        [HttpGet("{id:long}/{configType}")]
        public async Task<IActionResult> Get(long id)
        {
            var Reservation = await _ReservationQueryService.GetReservationAsync(id);
            if (Reservation == null)
                return NotFound();
            return Ok(Reservation);
        }

        [HttpPost("AddReservation")]
        public async Task<IActionResult> AddReservation([FromBody] ReservationDto Reservation)
        {
            await _ReservationCommandService.AddOrUpdateReservationAsync(Reservation);
            return CreatedAtAction(nameof(Get), new { id = Reservation.Id}, Reservation);
        }

        [HttpPut("UpdateReservation")]
        public async Task<IActionResult> UpdateReservation([FromBody] ReservationDto Reservation)
        {
            await _ReservationCommandService.AddOrUpdateReservationAsync(Reservation);
            //await _emailService.SendEmailAsync(new Application.DTOs.EmailRequestDto { Body = "Mi First Email", Subject = "TestEmail", To = "candysoft.roya@gmail.com" });

            return CreatedAtAction(nameof(Get), new { id = Reservation.Id }, Reservation);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _ReservationCommandService.DeleteReservationAsync(id);
            return Ok(id);
        }
    }
}
