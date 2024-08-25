using App.Domain.Entities;
using App.Domain.Enum;
using App.Domain.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.State
{
    public class SolicitedState : IReservationState
    {
        public void ProceedToNext(Reservation reservation)
        {
            reservation.SetState(new ConfirmedState());
        }

        public void Cancel(Reservation reservation)
        {
            reservation.SetState(new CanceledState());
        }
        public void JumpToState(Reservation reservation, IReservationState state)
        {
            reservation.SetState(state);
        }

        public AppoitmentStateType GetStateType() => AppoitmentStateType.Solicited;
    }

    public class ConfirmedState : IReservationState
    {
        public void ProceedToNext(Reservation reservation)
        {
            reservation.SetState(new ExecutedState());
        }

        public void Cancel(Reservation reservation)
        {
            reservation.SetState(new CanceledState());
        }

        public void JumpToState(Reservation reservation, IReservationState state)
        {
            reservation.SetState(state);
        }
        public AppoitmentStateType GetStateType() => AppoitmentStateType.Confirmed;
    }

    public class ExecutedState : IReservationState
    {
        public void ProceedToNext(Reservation reservation)
        {
            throw new InvalidOperationException("Executed state is the final state and cannot proceed to another state.");
        }

        public void Cancel(Reservation reservation)
        {
            throw new InvalidOperationException("Executed reservation cannot be canceled.");
        }
        public void JumpToState(Reservation reservation, IReservationState state)
        {
            reservation.SetState(state);
        }
        public AppoitmentStateType GetStateType() => AppoitmentStateType.Executed;
    }

    public class CanceledState : IReservationState
    {
        public void ProceedToNext(Reservation reservation)
        {
            throw new InvalidOperationException("Canceled state cannot proceed to another state.");
        }

        public void Cancel(Reservation reservation)
        {
            throw new InvalidOperationException("Reservation is already canceled.");
        }
        public void JumpToState(Reservation reservation, IReservationState state)
        {
            reservation.SetState(state);
        }
        public AppoitmentStateType GetStateType() => AppoitmentStateType.Canceled;
    }

}
