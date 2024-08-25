using AutoMapper;
using App.Application.DTOs;
using App.Domain.Entities;
using Newtonsoft.Json;

namespace App.Application.Mapper
{
    
    public class DeltaMappingProfile : Profile
    {
        public DeltaMappingProfile()
        {
            // Mapeos directos
            CreateMap<ConfigValue, ConfigValueDto>();
            CreateMap<Tattoo, TattooDto>();
            //CreateMap<Reservation, ReservationDto>();

            // Mapeos inversos
            CreateMap<ConfigValueDto, ConfigValue>();
            CreateMap<TattooDto, Tattoo>();
            //CreateMap<ReservationDto, Reservation>();

            // Mapeo de Reservation a ReservationDto
            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.ImagePaths, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<string[]>(src.ImagePathsJson)));

            // Mapeo de ReservationDto a Reservation
            CreateMap<ReservationDto, Reservation>()
                .ForMember(dest => dest.ImagePathsJson, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.ImagePaths)));
        }
    }
}
