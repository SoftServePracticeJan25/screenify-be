using Domain.DTOs.Data.StatisticsDtos;
using Domain.Helpers.QueryObject;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/statistic")]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        [HttpGet("best-selling-movies")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBestSellingMovies([FromQuery] StatisticDateQuery query)
        {
            var result = await _statisticService.GetBestSellingMoviesAsync(query);
            return Ok(result);
        }

        [HttpGet("best-rated-movies")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBestRatedMovies([FromQuery] StatisticDateQuery query)
        {
            var result = await _statisticService.GetBestRatedMoviesAsync(query);
            return Ok(result);
        }

        [HttpGet("most-popular-genres")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMostPopularGenres([FromQuery] StatisticDateQuery query)
        {
            var result = await _statisticService.GetMostPopularGenresAsync(query);
            return Ok(result);
        }
    }
}
