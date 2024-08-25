using App.Domain.Entities;
using App.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Interfases
{
    public interface IReservationState
    {
        void JumpToState(Reservation reservation, IReservationState state);
        void ProceedToNext(Reservation reservation);
        void Cancel(Reservation reservation);
        AppoitmentStateType GetStateType();
    }


}
