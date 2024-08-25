using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Enum;

namespace App.Domain.Entities
{
    public class Tattoo
    {
        [Key]
        [Required]
        public long Id { get; set; }
        public string TattooName { get; set; } = string.Empty; 
        public string TattooDescription{ get; set; } = string.Empty;
        public long? TattooStyleId { get; set; }
        public long? TattooBodyPartId { get; set; }
        public long? TattooGenreId { get; set; }
        public ConfigValue? TattooStyle { get; set; }
        public ConfigValue? TattooBodyPart { get; set; }
        public ConfigValue? TattooGenre { get; set; }
        public int Rating { get; set; }
        public int Order { get; set; }

        public string ImagePath { get; set; } = string.Empty;
        public string MiniatureImagePath { get; set; } = string.Empty;

        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();

        public void IncrementRating()
        {
            Rating++;
        }
    }

}
