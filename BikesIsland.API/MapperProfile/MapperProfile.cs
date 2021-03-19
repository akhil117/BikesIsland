using AutoMapper;
using BikesIsland.Models.Dto;
using BikesIsland.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikesIsland.Models.MapperProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<BikeReservationDto, BikeReservation>();
        }
    }
}
