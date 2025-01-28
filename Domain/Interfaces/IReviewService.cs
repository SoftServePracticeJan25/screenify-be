using Domain.DTOs.Data.ReviewDtos;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface IReviewService
    {
        Task<List<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        Task<Review> AddAsync(Review review);
        Task<Review?> UpdateAsync(int id, ReviewUpdateDto reviewUpdateDto);
        Task <Review?> DeleteAsync(int id);
        Task<bool> ReviewExist(int id);
    }
}