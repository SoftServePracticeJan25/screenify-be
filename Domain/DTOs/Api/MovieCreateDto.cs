using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs.Data.MovieActorDtos;

namespace Domain.DTOs.Api
{
    public class MovieCreateDto
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public List<int> GenreIds { get; set; }
        public List<MovieActorCreateDto> Actors { get; set; } // !
    }
}
