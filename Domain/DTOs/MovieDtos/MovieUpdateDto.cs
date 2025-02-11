using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.MovieDtos
{
    public class MovieUpdateDto
    {
        public string? Title { get; set; }
        public int? Duration { get; set; }
        public string? PosterUrl { get; set; }
        public List<int>? GenreIds { get; set; }
    }


}
