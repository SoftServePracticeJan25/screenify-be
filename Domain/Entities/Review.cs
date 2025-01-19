using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int Likes { get; set; }

        public int? MovieId { get; set; }
        public Movie? Movie { get; set; }
        public int? UserId { get; set; }
        //public User? User { get; set; } first solve Identity problem
    }
}
