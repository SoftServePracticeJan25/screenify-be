using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.ActorDtos;
using Domain.DTOs.Data.ActorRoleDtos;
using Domain.DTOs.Data.ReviewDtos;
using Domain.DTOs.Data.MovieActorDtos;
using Domain.DTOs.Data.TransactionDtos;
using Domain.DTOs.Data.Transactiondtos;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs.Data.TicketDtos;

namespace Infrastructure.MappingProfiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Actor, ActorDto>().ReverseMap();
            CreateMap<Actor, ActorUpdateDto>().ReverseMap();
            CreateMap<Actor, ActorCreateDto>().ReverseMap();
            CreateMap<Actor, ActorReadDto>().ReverseMap();

            CreateMap<ActorRole, ActorRoleDto>().ReverseMap();
            CreateMap<ActorRole, ActorRoleUpdateDto>().ReverseMap();
            CreateMap<ActorRole, ActorRoleCreateDto>().ReverseMap(); 
            CreateMap<ActorRole, ActorRoleReadDto>().ReverseMap();    


            CreateMap<CinemaType, CinemaTypeDto>();
            CreateMap<Genre, GenreDto>();

            CreateMap<MovieActor, MovieActorDto>().ReverseMap();
            CreateMap<MovieActor, MovieActorCreateDto>().ReverseMap();
            CreateMap<MovieActor, MovieActorUpdateDto>().ReverseMap();
            CreateMap<MovieActor, MovieActorReadDto>().ReverseMap();

            CreateMap<Movie, MovieDto>();
            CreateMap<MovieGenre, MovieGenreDto>();

            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<Review, ReviewUpdateDto>().ReverseMap();
            CreateMap<Review, ReviewCreateDto>().ReverseMap();
            CreateMap<Review, ReviewReadDto>()
            .ForMember(dest => dest.MadeBy, opt => opt.MapFrom(src => src.AppUser != null ? src.AppUser.UserName : "Unknown"));

            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<Transaction, TransactionCreateDto>().ReverseMap();
            CreateMap<Transaction, TransactionUpdateDto>().ReverseMap();
            CreateMap<Transaction, TransactionReadDto>().ReverseMap();
            
            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<Ticket, TicketCreateDto>().ReverseMap();
            CreateMap<Ticket, TicketUpdateDto>().ReverseMap();
            CreateMap<Ticket, TicketReadDto>().ReverseMap();

            CreateMap<Room, RoomDto>();
            CreateMap<Session, SessionDto>();
        }
    }
}
