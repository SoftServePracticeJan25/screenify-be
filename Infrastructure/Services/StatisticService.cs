using AutoMapper;
using Domain.DTOs.Data.StatisticsDtos;
using Domain.Entities;
using Domain.Helpers.QueryObject;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class StatisticService(MovieDbContext context) : IStatisticService
    {
        public async Task<List<StatisticBestSoldMoviesDto>> GetBestSellingMoviesAsync(StatisticDateQuery query)
        {
            var tickets = context.Tickets
                .Include(t => t.Session)
                .ThenInclude(s => s!.Movie)
                .AsQueryable();

            if (query.StartDate.HasValue)
                tickets = tickets.Where(t => t.Transaction!.CreationTime.Date >= query.StartDate.Value.Date);

            if (query.EndDate.HasValue)
                tickets = tickets.Where(t => t.Transaction!.CreationTime.Date <= query.EndDate.Value.Date);

            var bestSellingMovies = await tickets
                .GroupBy(t => t.Session!.MovieId)
                .Select(g => new StatisticBestSoldMoviesDto
                {
                    MovieId = g.Key!.Value,
                    TicketsSold = g.Count()
                })
                .OrderByDescending(x => x.TicketsSold)
                .ToListAsync();

            return bestSellingMovies;
        }

        public async Task<List<StatisticMostRatedMoviesDto>> GetBestRatedMoviesAsync(StatisticDateQuery query)
        {
            var reviews = context.Reviews
                .Include(r => r.Movie)
                .AsQueryable();

            if (query.StartDate.HasValue)
                reviews = reviews.Where(r => r.CreationTime.Date >= query.StartDate.Value.Date);

            if (query.EndDate.HasValue)
                reviews = reviews.Where(r => r.CreationTime.Date <= query.EndDate.Value.Date);

            var bestRatedMovies = await reviews
                .GroupBy(r => r.MovieId)
                .Select(g => new StatisticMostRatedMoviesDto
                {
                    MovieId = g.Key!.Value,
                    Rating = Math.Round(g.Average(r => r.Rating), 1)
                })
                .OrderByDescending(x => x.Rating)
                .ToListAsync();

            return bestRatedMovies;
        }

        public async Task<List<StatisticMostPopularGenre>> GetMostPopularGenresAsync(StatisticDateQuery query)
        {
            var tickets = context.Tickets
                .Include(t => t.Session)
                .ThenInclude(s => s!.Movie)
                .ThenInclude(m => m!.MovieGenres)
                .ThenInclude(mg => mg.Genre)
                .AsQueryable();

            if (query.StartDate.HasValue)
                tickets = tickets.Where(t => t.Transaction!.CreationTime.Date >= query.StartDate.Value.Date);

            if (query.EndDate.HasValue)
                tickets = tickets.Where(t => t.Transaction!.CreationTime.Date <= query.EndDate.Value.Date);

            var genrePopularity = await tickets
                .SelectMany(t => t.Session!.Movie!.MovieGenres)
                .GroupBy(mg => mg.GenreId)
                .Select(g => new StatisticMostPopularGenre
                {
                    GenreId = g.Key,
                    TicketsSold = g.Count()
                })
                .OrderByDescending(x => x.TicketsSold)
                .ToListAsync();

            return genrePopularity;
        }
    }
}
