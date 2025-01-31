using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

public class MovieDbContext: IdentityDbContext<AppUser>
{
    public MovieDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        
    }
    
    public DbSet<Movie> Movies { get; set; }
    public DbSet<MovieActor> MovieActors { get; set; } // join table
    public DbSet<Actor> Actors { get; set; }
    public DbSet<ActorRole> ActorRoles { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<MovieGenre> MovieGenres { get; set; } // join table
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<CinemaType> CinemaTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            }
        };
        modelBuilder.Entity<IdentityRole>().HasData(roles);

        modelBuilder.Entity<Review>()
            .HasOne(u => u.AppUser)
            .WithMany(r => r.Reviews)
            .HasForeignKey(p => p.AppUserId);

        modelBuilder.Entity<Transaction>()
            .HasOne(u => u.AppUser)
            .WithMany(t => t.Transactions)
            .HasForeignKey(p => p.AppUserId);

        // Composite Key
        modelBuilder.Entity<MovieActor>()
            .HasKey(e => new { e.MovieId, e.ActorId, e.ActorRoleId });

        modelBuilder.Entity<MovieGenre>()
            .HasKey(e => new { e.MovieId, e.GenreId });
    }

}