using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.Abstractions.Implementations;
using App.Domain.Entities;
using App.Domain.Enum;

namespace App.Application.DTOs
{
    public class TattooDto:IDeepCloneable<TattooDto>
    {
        public long Id { get; set; }
        public string TattooName { get; set; } = string.Empty;
        public string TattooDescription { get; set; } = string.Empty;
        public long? TattooStyleId { get; set; }
        public long? TattooBodyPartId { get; set; }
        public long? TattooGenreId { get; set; }
        public bool IsFavorite { get; set; }

        public ConfigValueDto? TattooStyle { get; set; }
        public ConfigValueDto? TattooBodyPart { get; set; }
        public ConfigValueDto? TattooGenre { get; set; }
        public int Order { get; set; }
        public int Rating { get; set; }

        public string ImagePath { get; set; } = string.Empty;
        public string MiniatureImagePath { get; set; } = string.Empty;

        public TattooDto DeepClone()
        {
            return new TattooDto
            {
                Id = this.Id,
                TattooName = this.TattooName,
                TattooDescription = this.TattooDescription ,
                //ConfigValues = this.ConfigValues.,
                ///TODO :Buscar como clonar listados
                ImagePath = this.ImagePath
            };
        }
    }
}
