using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.ActorDtos;
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
            CreateMap<Actor, ActorUpdateDto>();
            CreateMap<Actor, ActorCreateDto>();

            CreateMap<ActorRole, ActorRoleDto>();
            CreateMap<CinemaType, CinemaTypeDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<MovieActor, MovieActorDto>();
            CreateMap<Movie, MovieDto>();
            CreateMap<MovieGenre, MovieGenreDto>();
            CreateMap<Review, ReviewDto>();
            CreateMap<Room, RoomDto>();
            CreateMap<Session, SessionDto>();
            CreateMap<Ticket, TicketDto>();
        }
    }
}
