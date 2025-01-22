using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Data
{
    public class MovieGenreDto
    {
        public int? MovieId { get; set; }
        public int? GenreId { get; set; }
    }
}
