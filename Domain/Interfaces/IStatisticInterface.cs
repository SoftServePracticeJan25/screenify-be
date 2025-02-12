using Domain.DTOs.Data.StatisticsDtos;
using Domain.Helpers.QueryObject;

namespace Domain.Interfaces
{
    public interface IStatisticService
    {
        Task<List<StatisticBestSoldMoviesDto>> GetBestSellingMoviesAsync(StatisticDateQuery query);
        Task<List<StatisticMostRatedMoviesDto>> GetBestRatedMoviesAsync(StatisticDateQuery query);
        Task<List<StatisticMostPopularGenre>> GetMostPopularGenresAsync(StatisticDateQuery query);
    }
}
