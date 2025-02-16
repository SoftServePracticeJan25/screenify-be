using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace screenify_be_dev_MovieController_test;

public class MoviesControllerTests
{
    //private readonly Mock<IMovieRepository> _mockMovieRepository;
    //private readonly MoviesController _controller;

    //    public MoviesControllerTests()
    //    {
    //        _mockMovieRepository = new Mock<IMovieRepository>();
    //        _controller = new MoviesController(_mockMovieRepository.Object);
    //    }

    //    [Fact]
    //    public async Task GetAllReturnsOkResultWithMovies()
    //    {
    //        var movies = new List<Movie>
    //        {
    //            new Movie { Id = 1, Title = "Arcane" },
    //            new Movie { Id = 2, Title = "Oblivion" }
    //        };
    //        _mockMovieRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(movies);
            
    //        var result = await _controller.GetAll();
            
    //        var okResult = Assert.IsType<OkObjectResult>(result); 
    //        var returnMovies = Assert.IsAssignableFrom<List<Movie>>(okResult.Value); 
    //        Assert.Equal(2, returnMovies.Count); 
    //        Assert.Equal("Arcane", returnMovies[0].Title); 
    //    }

    //    [Fact]
    //    public async Task GetByIdReturnsOkResultWithMovie()
    //    {
    //        var movie = new Movie { Id = 1, Title = "Arcane" };
    //        _mockMovieRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(movie);
            
    //        var result = await _controller.GetById(1);
            
    //        var okResult = Assert.IsType<OkObjectResult>(result); 
    //        var returnMovie = Assert.IsAssignableFrom<Movie>(okResult.Value); 
    //        Assert.Equal("Arcane", returnMovie.Title); 
    //    }

    //    [Fact]
    //    public async Task CreateReturnsCreatedAtActionResultWithMovie()
    //    {
    //        var movie = new Movie { Id = 1, Title = "Arcane" };
    //        _mockMovieRepository.Setup(repo => repo.AddAsync(movie)).Returns(Task.CompletedTask); 
            
    //        var result = await _controller.Create(movie);
            
    //        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result); 
    //        var returnMovie = Assert.IsAssignableFrom<Movie>(createdAtActionResult.Value); 
    //        Assert.Equal(movie.Id, returnMovie.Id); 
    //        Assert.Equal(movie.Title, returnMovie.Title); 
    //    }
}