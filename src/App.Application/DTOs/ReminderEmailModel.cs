using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs
{
    public class ReminderEmailModel
    {
        public string ClientName { get; set; }
        public DateTime ReservationDate { get; set; }
        public string Description { get; set; }
        public string ProviderName { get; set; } // Nuevo campo para el nombre del proveedor
        public string PhoneNumber { get; set; } // Nueva propiedad para el teléfono
        public string Address { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string TikTok { get; set; }
        public double Latitude { get; set; } // Nueva propiedad para la latitud
        public double Longitude { get; set; }
    }
}
