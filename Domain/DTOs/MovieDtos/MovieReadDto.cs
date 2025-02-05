using Domain.DTOs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs.Data.MovieActorDtos;
using Domain.DTOs.Data.GenreDtos;

namespace Domain.DTOs.Api
{
    public class MovieReadDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int Duration { get; set; }
        public string? PosterUrl { get; set; }
        public List<GenreDto>? Genres { get; set; }
        public List<MovieActorReadListDto>? Actors { get; set; }
    }
}
