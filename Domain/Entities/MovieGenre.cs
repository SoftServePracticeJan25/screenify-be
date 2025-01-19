using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MovieGenre
    {
        public int? MovieId { get; set; }
        public Movie? Movie { get; set; }
        public int? GenreId { get; set; }
        public Genre? Genre { get; set; }
    }
}
