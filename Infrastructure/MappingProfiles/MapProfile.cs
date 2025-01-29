using AutoMapper;
using Domain.DTOs.Data;
using Domain.DTOs.Api;
using Domain.Entities;

namespace Infrastructure.MappingProfiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Actor, ActorDto>();

            CreateMap<Room, RoomReadDto>();
            CreateMap<RoomCreateDto, Room>();

            CreateMap<ActorRole, ActorRoleDto>();

            CreateMap<CinemaType, CinemaTypeDto>();

            CreateMap<Genre, GenreDto>().ReverseMap();

            CreateMap<MovieActor, MovieActorDto>()
                .ForMember(dest => dest.Actor, opt => opt.MapFrom(src => new ActorDto
                {
                    Name = src.Actor.Name,
                    Bio = src.Actor.Bio,
                    BirthDate = src.Actor.BirthDate,
                    PhotoUrl = src.Actor.PhotoUrl
                }))
                .ForMember(dest => dest.CharacterName, opt => opt.MapFrom(src => src.CharacterName))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.ActorRole.RoleName));

            CreateMap<MovieCreateDto, Movie>()
                .ForMember(dest => dest.MovieGenres, opt => opt.MapFrom(src =>
                    src.GenreIds.Select(genreId => new MovieGenre { GenreId = genreId })))
                .ForMember(dest => dest.MovieActors, opt => opt.MapFrom(src =>
                    src.Actors.Select(actor => new MovieActor
                    {
                        ActorId = actor.ActorId,
                        ActorRoleId = actor.RoleId,
                        CharacterName = actor.CharacterName
                    })));

            CreateMap<Movie, MovieReadDto>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                    src.MovieGenres
                        .Where(mg => mg.Genre != null)
                        .Select(mg => new GenreDto
                        {
                            Id = mg.GenreId,
                            Name = mg.Genre.Name
                        })))
                .ForMember(dest => dest.Actors, opt => opt.MapFrom(src =>
                    src.MovieActors.Select(ma => new MovieActorDto
                    {
                        Actor = new ActorDto
                        {
                            Name = ma.Actor.Name,
                            Bio = ma.Actor.Bio,
                            BirthDate = ma.Actor.BirthDate,
                            PhotoUrl = ma.Actor.PhotoUrl
                        },
                        CharacterName = ma.CharacterName,
                        RoleName = ma.ActorRole.RoleName
                    })));

            CreateMap<Session, SessionDto>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.Room, opt => opt.Ignore())
                .ForMember(dest => dest.Tickets, opt => opt.Ignore());

            CreateMap<GenreCreateDto, Genre>(); // ready to merge
        }
    }
}
