using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.ActorDtos;
using Domain.DTOs.Data.ActorRoleDtos;
using Domain.DTOs.Data.ReviewDtos;
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
            CreateMap<Actor, ActorCreateDto>().ReverseMap();

            CreateMap<ActorRole, ActorRoleDto>().ReverseMap();
            CreateMap<ActorRole, ActorRoleUpdateDto>().ReverseMap();
            CreateMap<ActorRole, ActorRoleCreateDto>().ReverseMap();    

            CreateMap<CinemaType, CinemaTypeDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<MovieActor, MovieActorDto>();
            CreateMap<Movie, MovieDto>();
            CreateMap<MovieGenre, MovieGenreDto>();

            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<Review, ReviewUpdateDto>().ReverseMap();
            CreateMap<Review, ReviewCreateDto>().ReverseMap();
            CreateMap<Review, ReviewViewDto>().ReverseMap();

            CreateMap<Room, RoomDto>();
            CreateMap<Session, SessionDto>();
            CreateMap<Ticket, TicketDto>();
        }
    }
}
