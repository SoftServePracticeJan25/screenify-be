namespace Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }


        public List<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
        public List<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<Session> Sessions { get; set; } = new List<Session>();
    }
}
