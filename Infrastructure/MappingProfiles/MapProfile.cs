using AutoMapper;
using Domain.DTOs.Data;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MappingProfiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Actor, ActorDto>();
            CreateMap<ActorRole, ActorRoleDto>();
            CreateMap<CinemaType, CinemaTypeDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<MovieActor, ActorDto>();
            CreateMap<Movie, ActorDto>();
            CreateMap<MovieGenre, ActorDto>();
            CreateMap<Review, ActorDto>();
            CreateMap<Room, ActorDto>();
            CreateMap<Session, ActorDto>();
            CreateMap<Ticket, ActorDto>();
        }
    }
}
