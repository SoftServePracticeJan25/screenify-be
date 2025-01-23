namespace Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }


        public List<MovieGenre> MovieGenres { get; set; } = [];
        public List<MovieActor> MovieActors { get; set; } =[];
        public List<Review> Reviews { get; set; } = [];
        public List<Session> Sessions { get; set; } = [];
    }
}
