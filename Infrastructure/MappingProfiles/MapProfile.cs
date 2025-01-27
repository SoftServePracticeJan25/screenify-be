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
            CreateMap<ActorRole, ActorRoleDto>();           
            CreateMap<CinemaType, CinemaTypeDto>();          
            CreateMap<Genre, GenreDto>();
           
            CreateMap<MovieActor, ActorDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Actor.Name))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Actor.Bio))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Actor.BirthDate))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Actor.PhotoUrl))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.ActorRole));

            
            CreateMap<MovieCreateDto, Movie>()
                .ForMember(dest => dest.MovieGenres, opt => opt.MapFrom(src =>
                    src.GenreIds.Select(genreId => new MovieGenre { GenreId = genreId })))
                .ForMember(dest => dest.MovieActors, opt => opt.MapFrom(src =>
                    src.Actors.Select(actor => new MovieActor
                    {
                        ActorId = actor.ActorId,
                        ActorRoleId = actor.RoleId
                    })));

            
            CreateMap<Movie, MovieReadDto>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                    src.MovieGenres.Select(mg => new GenreDto
                    {
                        Id = mg.GenreId,
                        Name = mg.Genre.Name
                    })))
                .ForMember(dest => dest.Actors, opt => opt.MapFrom(src =>
                    src.MovieActors.Select(ma => new ActorDto
                    {
                        Name = ma.Actor.Name,
                        Bio = ma.Actor.Bio,
                        BirthDate = ma.Actor.BirthDate,
                        PhotoUrl = ma.Actor.PhotoUrl,
                        Role = new ActorRoleDto
                        {
                            RoleName = ma.ActorRole.RoleName
                        }
                    })));

            
            CreateMap<Session, SessionDto>();

            CreateMap<SessionDto, Session>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id
                .ForMember(dest => dest.Movie, opt => opt.Ignore()) // Ignore Navigation
                .ForMember(dest => dest.Room, opt => opt.Ignore())
                .ForMember(dest => dest.Tickets, opt => opt.Ignore());
        }
    }
}
