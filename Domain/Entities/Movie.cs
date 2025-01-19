namespace Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; } // do we really need this?

        public List<Genre> Genres { get; set; } = new List<Genre>();
        public List<Actor> Actors { get; set; } = new List<Actor>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<Session> Sessions { get; set; } = new List<Session>();
    }
}
