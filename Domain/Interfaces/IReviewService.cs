using Domain.DTOs.Data.ReviewDtos;
using Domain.Entities;
using Domain.Helpers.QueryObject;
namespace Domain.Interfaces
{
    public interface IReviewService
    {
        Task<List<ReviewReadDto>> GetAllAsync(ReviewQueryObject queryObject);
        Task<ReviewReadDto?> GetByIdAsync(int id);
        Task<ReviewReadDto> AddAsync(Review review);
        Task<ReviewReadDto?> UpdateAsync(int id, ReviewUpdateDto reviewUpdateDto);
        Task <ReviewReadDto?> DeleteAsync(int id);
        Task<bool> ReviewExist(int movieId, string appUserId);
    }
}