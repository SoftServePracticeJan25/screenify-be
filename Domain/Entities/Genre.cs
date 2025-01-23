using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;

        public List<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    }
}
