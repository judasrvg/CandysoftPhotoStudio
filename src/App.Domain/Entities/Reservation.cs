using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using App.Domain.Enum;
using App.Domain.Interfases;
using App.Domain.State;
using Newtonsoft.Json.Linq;

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

        // Nueva propiedad para almacenar las ofertas en formato JSON
        public string OffersJson { get; set; } = "[]";

        // Nueva propiedad para almacenar el total de la reserva
        public decimal TotalAmount { get; set; }

        // Propiedad no mapeada para manejar las rutas de las imágenes
        [NotMapped]
        public List<string> ImagePaths
        {
            get => JsonConvert.DeserializeObject<List<string>>(ImagePathsJson);
            set => ImagePathsJson = JsonConvert.SerializeObject(value);
        }

        // Propiedad no mapeada para manejar las ofertas en una lista
        [NotMapped]
        public List<ConfigValue> Offers
        {
            get => JsonConvert.DeserializeObject<List<ConfigValue>>(OffersJson);
            set =>  CalculateTotalAmount(value);
        }

        private IReservationState _currentState;
        public string Details { get; set; } = string.Empty;
        public bool Notified { get; set; }

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

        public AppoitmentStateType CurrentStateType { get; private set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public Reservation()
        {
            // Inicializar el estado inicial de la orden
            CurrentState = new SolicitedState();
        }

        // Métodos para cambiar el estado, delegando la lógica al estado actual
        public void ProceedToNext()
        {
            CurrentState.ProceedToNext(this);
            GenerateTransactionIfNeeded();
        }

        public void Cancel()
        {
            CurrentState.Cancel(this);
        }

        public void JumpToState(IReservationState state)
        {
            CurrentState = state;
            GenerateTransactionIfNeeded();

        }

        // Método para establecer estados específicos internamente
        internal void SetState(IReservationState state)
        {
            CurrentState = state;
        }

        // Método para calcular el total de la reserva en base a las ofertas
        public void CalculateTotalAmount(List<ConfigValue> value)
        {
            if (Offers != null)
            {
                OffersJson = JsonConvert.SerializeObject(value);
                TotalAmount = Offers.Sum(offer => offer.PriceValue);
            }
        }

        private void GenerateTransactionIfNeeded()
        {
            if (CurrentStateType == AppoitmentStateType.Confirmed)
            {
                GenerateTransaction();
            }
        }

        // Método para generar una transacción
        private void GenerateTransaction()
        {
            if (TotalAmount > 0)
            {
                var transaction = new Transaction
                {
                    ReservationId = Id,
                    TransactionDate = DateTime.UtcNow,
                    Quantity = 1, // O el valor adecuado
                    TransactionType = TransactionType.Income, // O Expense según sea el caso
                    TotalAmount = TotalAmount
                };
                Transactions.Add(transaction);
            }
        }
    }

}
