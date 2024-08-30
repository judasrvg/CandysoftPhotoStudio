using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Application.Abstractions.Implementations;
using App.Domain.Entities;
using App.Domain.Enum;

namespace App.Application.DTOs
{
    public class ReservationDto
    {
        public long Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string ClientPhone { get; set; } = string.Empty;
        public int BudgetAmount { get; set; }
        public bool IsWithColor { get; set; }
        public bool IsCoverUp { get; set; }
        public DateTime ReservationDateStart { get; set; }
        public DateTime ReservationDateEnd { get; set; }
        public string[] ImagePaths { get; set; } = Array.Empty<string>();
        public AppoitmentStateType CurrentStateType { get; set; }
        public string Details { get; set; } = string.Empty;
        public bool Notified { get; set; }
        public string Lang { get; set; } = "es";

        public List<ConfigValueDto> Offers { get; set; } = new List<ConfigValueDto>();

        public decimal TotalAmount { get; set; }
    }

}
