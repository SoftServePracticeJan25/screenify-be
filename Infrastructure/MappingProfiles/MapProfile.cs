using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Data.ActorDtos;
using Domain.DTOs.Data.ActorRoleDtos;
using Domain.DTOs.Data.ReviewDtos;
using Domain.DTOs.Data.MovieActorDtos;
using Domain.DTOs.Data.TransactionDtos;
using Domain.DTOs.Data.Transactiondtos;
using Domain.Entities;
using System.Linq;
using Domain.DTOs.Data.TicketDtos;
using Domain.DTOs.Api;
using Domain.DTOs.Data.RoomDtos;
using Domain.DTOs.Data.GenreDtos;
using Domain.DTOs.Data.CinemaTypeDtos;
using Domain.DTOs.Data.MovieGenresDtos;
using Domain.DTOs.Data.SessionDtos;
using Domain.DTOs.Account;
using Domain.DTOs.MovieDtos;

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

            CreateMap<MovieGenre, GenreDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.GenreId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Genre != null ? src.Genre.Name : "Unknown"));

            CreateMap<CinemaType, CinemaTypeDto>();
            CreateMap<Genre, GenreDto>().ReverseMap();

            CreateMap<MovieActor, MovieActorDto>().ReverseMap();

            CreateMap<MovieActor, MovieActorReadListDto>()
                .ForMember(dest => dest.Actor, opt => opt.MapFrom(src =>
                    src.Actor != null
                        ? new ActorDto
                        {
                            Name = src.Actor.Name ?? "Unknown",
                            BirthDate = src.Actor.BirthDate,
                            PhotoUrl = src.Actor.PhotoUrl
                        }
                        : null))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src =>
                    src.ActorRole != null
                        ? new ActorRoleDto { RoleName = src.ActorRole.RoleName ?? "Unknown" }
                        : null))
                .ForMember(dest => dest.CharacterName, opt => opt.MapFrom(src =>
                    src.CharacterName ?? "Unknown"));

            CreateMap<MovieActorCreateDto, MovieActor>().ReverseMap();
            CreateMap<MovieActor, MovieActorUpdateDto>().ReverseMap();
            CreateMap<MovieActor, MovieActorReadDto>().ReverseMap();
            CreateMap<MovieActorCreateListDto, MovieActor>().ReverseMap();

            CreateMap<MovieCreateDto, Movie>()
                .ForMember(dest => dest.MovieGenres, opt => opt.MapFrom(src =>
                    (src.GenreIds ?? new List<int>()).Select(genreId => new MovieGenre { GenreId = genreId })))
                .ForMember(dest => dest.MovieActors, opt => opt.MapFrom(src =>
                    (src.Actors ?? new List<MovieActorCreateListDto>()).Select(actor => new MovieActor
                    {
                        ActorId = actor.ActorId,
                        ActorRoleId = actor.ActorRoleId,
                        CharacterName = actor.CharacterName
                    })));

            CreateMap<MovieUpdateDto, Movie>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Movie, MovieReadDto>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                    src.MovieGenres != null
                        ? src.MovieGenres.Select(mg => mg.Genre).Where(g => g != null).Select(g => new GenreDto
                        {
                            Id = g.Id,
                            Name = g.Name
                        }).ToList()
                        : new List<GenreDto>()))
                .ForMember(dest => dest.Actors, opt => opt.MapFrom(src =>
                    src.MovieActors != null
                        ? src.MovieActors.Select(ma => new MovieActorReadListDto
                        {
                            Actor = ma.Actor != null
                                ? new ActorDto
                                {
                                    Name = ma.Actor.Name ?? "Unknown",
                                    BirthDate = ma.Actor.BirthDate,
                                    PhotoUrl = ma.Actor.PhotoUrl
                                }
                                : null,
                            RoleName = ma.ActorRole != null
                                ? new ActorRoleDto { RoleName = ma.ActorRole.RoleName ?? "Unknown" }
                                : null,
                            CharacterName = ma.CharacterName ?? "Unknown"
                        }).ToList()
                        : new List<MovieActorReadListDto>()));
 
            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<Review, ReviewUpdateDto>().ReverseMap();
            CreateMap<Review, ReviewCreateDto>().ReverseMap();
            CreateMap<Review, ReviewReadDto>()
                .ForMember(dest => dest.MadeBy, opt => opt.MapFrom(src => src.AppUser != null ? src.AppUser.UserName ?? "Unknown" : "Unknown"))
                .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.AppUserId))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.AppUser != null ? src.AppUser.PhotoUrl : null));

            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<Transaction, TransactionCreateDto>().ReverseMap();
            CreateMap<Transaction, TransactionUpdateDto>().ReverseMap();
            CreateMap<Transaction, TransactionReadDto>().ReverseMap();

            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<Ticket, TicketCreateDto>().ReverseMap();
            CreateMap<Ticket, TicketUpdateDto>().ReverseMap();
            CreateMap<Ticket, TicketReadDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Transaction.AppUser.Id))  // маппим UserId
            .ForMember(dest => dest.TransactionTime, opt => opt.MapFrom(src => src.Transaction.CreationTime)) // маппим время транзакции
            .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Session.Movie.Id)) // маппим MovieId
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Session.Movie.Title)) // маппим Title
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Session.Movie.Duration)) // маппим Duration
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Session.StartTime)) // маппим StartTime
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Session.Price)) // маппим Price
            .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Session.Room.Name)); // маппим RoomName
            CreateMap<Ticket, TicketFileDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Session != null && src.Session.Movie != null ? src.Session.Movie.Title : "Unknown"))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Session != null && src.Session.Room != null ? src.Session.Room.Name : "Unknown"))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Session != null ? src.Session.StartTime : DateTime.MinValue))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Session != null ? src.Session.Price : 0));
            CreateMap<Ticket, TicketNotifDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => 
                src.Session != null && src.Session.Movie != null ? src.Session.Movie.Title : "Unknown"))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => 
                src.Session != null ? src.Session.StartTime : DateTime.MinValue))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => 
                src.Session != null && src.Session.Movie != null && src.Session.Movie.Duration > 0 
                    ? src.Session.StartTime.AddMinutes(src.Session.Movie.Duration) 
                    : src.Session.StartTime.AddMinutes(120))) // Если нет Duration, то 2 часа по умолчанию
            .ForMember(dest => dest.Adress, opt => opt.MapFrom(_ => "Shevchenka Ave, 1Ф, Odesa, Odesa Oblast, 65000"));

            CreateMap<Room, RoomReadDto>()
             .ForMember(dest => dest.CinemaTypeName, opt => opt.MapFrom(src => src.CinemaType.Name)); 

            CreateMap<RoomCreateDto, Room>();
            CreateMap<Room, RoomDto>();

            CreateMap<Session, SessionDto>()
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room.Name)); 

            CreateMap<SessionCreateDto, Session>() 
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.Room, opt => opt.Ignore())
                .ForMember(dest => dest.Tickets, opt => opt.Ignore());



            CreateMap<GenreCreateDto, Genre>();
            CreateMap<CinemaType, CinemaTypeReadDto>().ReverseMap();
            CreateMap<CinemaTypeDto, CinemaType>().ReverseMap();

            CreateMap<AppUser, RegisterDto>().ReverseMap();
            CreateMap<AppUser, UserInfoDto>().ReverseMap();
        }
    }

}