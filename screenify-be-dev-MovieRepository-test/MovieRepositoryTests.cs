using Domain.Entities;
using Domain.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace screenify_be_dev_MovieRepository_test;

public class MovieRepositoryTests
{
    private readonly Mock<IMovieRepository> _mockMovieRepo;

        public MovieRepositoryTests()
        {
            _mockMovieRepo = new Mock<IMovieRepository>();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfMovies()
        {
            var movies = new List<Movie>
            {
                new Movie { Id = 1, Title = "Arcane" },
                new Movie { Id = 2, Title = "Oblivion" }
            };
            _mockMovieRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(movies);
            
            var result = await _mockMovieRepo.Object.GetAllAsync();
            
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Arcane", result[0].Title);
            Assert.Equal("Oblivion", result[1].Title);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMovie_WhenMovieExists()
        {
            var movie = new Movie { Id = 1, Title = "Arcane" };
            _mockMovieRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(movie);
            
            var result = await _mockMovieRepo.Object.GetByIdAsync(1);
            
            Assert.NotNull(result);
            Assert.Equal("Arcane", result?.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenMovieDoesNotExist()
        {
            _mockMovieRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Movie?)null);
            
            var result = await _mockMovieRepo.Object.GetByIdAsync(99);
            
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_AddsMovie()
        {
            var movie = new Movie { Id = 1, Title = "New Movie" };
            
            await _mockMovieRepo.Object.AddAsync(movie);
            
            _mockMovieRepo.Verify(repo => repo.AddAsync(It.Is<Movie>(m => m.Id == movie.Id && m.Title == movie.Title)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesMovie()
        {
            var movie = new Movie { Id = 1, Title = "Updated Movie" };
            
            await _mockMovieRepo.Object.UpdateAsync(movie);
            
            _mockMovieRepo.Verify(repo => repo.UpdateAsync(It.Is<Movie>(m => m.Id == movie.Id && m.Title == movie.Title)), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesMovie()
        {
            var movieId = 1;
            
            await _mockMovieRepo.Object.DeleteAsync(movieId);
            
            _mockMovieRepo.Verify(repo => repo.DeleteAsync(It.Is<int>(id => id == movieId)), Times.Once);
        }
}