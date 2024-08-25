using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Enum;
using App.Domain.Interfases;
using App.Domain.State;
using Newtonsoft.Json;

namespace App.Domain.Entities
{
    public class Reservation
    {
        [Key]
        [Required]
        public long Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string ClientPhone { get; set; } = string.Empty;
        public int BudgetAmount { get; set; } 
        public bool IsWithColor { get; set; }
        public bool IsCoverUp { get; set; }
        public DateTime ReservationDateStart { get; set; }
        public DateTime ReservationDateEnd { get; set; }
        public string ImagePathsJson { get; set; } = "[]";
        public string Lang { get; set; } = "en";

        [NotMapped]
        public List<string> ImagePaths
        {
            get => JsonConvert.DeserializeObject<List<string>>(ImagePathsJson);
            set => ImagePathsJson = JsonConvert.SerializeObject(value);
        }

        //public Tattoo? Tattoo { get; set; } = new Tattoo();

        // Campo privado para almacenar el estado actual
        private IReservationState _currentState;
        public string Details { get; set; } = string.Empty;

        public bool Notified { get; set; }
        // Propiedad pública para acceder al estado actual. No se mapea a la base de datos.
        [NotMapped]
        public IReservationState CurrentState
        {
            get => _currentState;
            private set
            {
                _currentState = value;
                CurrentStateType = _currentState.GetStateType();
            }
        }

        // Propiedad para almacenar el tipo de estado actual
        public AppoitmentStateType CurrentStateType { get; private set; }

        public Reservation()
        {
            // Inicializar el estado inicial de la orden
            CurrentState = new SolicitedState();
        }

        // Métodos para cambiar el estado, delegando la lógica al estado actual
        public void ProceedToNext()
        {
            CurrentState.ProceedToNext(this);
        }

        public void Cancel()
        {
            CurrentState.Cancel(this);
        }

        public void JumpToState(IReservationState state)
        {
            CurrentState.JumpToState(this, state);
        }

        // Método para establecer estados específicos internamente
        internal void SetState(IReservationState state)
        {
            CurrentState = state;
        }
    }



}
