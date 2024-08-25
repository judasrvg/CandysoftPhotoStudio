using App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs
{
    public class FormFilterGetTattoos
    {
        public string TattooName { get; set; } = string.Empty;
        public long? TattooStyleId { get; set; }
        public long? TattooBodyPartId { get; set; }
        public long? TattooGenreId { get; set; }
    }
}
