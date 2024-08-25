using App.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs
{
    public class AppointmentCreatedEvent
    {
        public ReservationDto Reservation { get; }

        public AppointmentCreatedEvent(ReservationDto reservation)
        {
            Reservation = reservation;
        }
    }

}
