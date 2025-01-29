using Domain.DTOs.Data.ReviewDtos;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface IReviewService
    {
        Task<List<ReviewReadDto>> GetAllAsync();
        Task<ReviewReadDto?> GetByIdAsync(int id);
        Task<ReviewReadDto> AddAsync(Review review);
        Task<ReviewReadDto?> UpdateAsync(int id, ReviewUpdateDto reviewUpdateDto);
        Task <ReviewReadDto?> DeleteAsync(int id);
        Task<bool> ReviewExist(int id);
    }
}