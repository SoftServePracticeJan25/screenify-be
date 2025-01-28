using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs.Data;
using Domain.DTOs.Data.ActorDtos;
using Domain.DTOs.Data.ReviewDtos;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly MovieDbContext _context;
        public ReviewService(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<Review> AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review?> DeleteAsync(int id)
        {
            var reviewModel = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);

            if (reviewModel == null)
            {
                return null;
            }

            _context.Reviews.Remove(reviewModel);
            await _context.SaveChangesAsync();
            return reviewModel;
        }

        public async Task<List<Review>> GetAllAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Review?> UpdateAsync(int id, ReviewUpdateDto reviewUpdateDto)
        {
            var existingReview = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);

            if(existingReview == null)
            {
                return null;
            }

            existingReview.Rating = reviewUpdateDto.Rating;
            existingReview.Comment = reviewUpdateDto.Comment;
            existingReview.Likes = reviewUpdateDto.Likes;

            await _context.SaveChangesAsync();

            return existingReview;
        }

        public async Task<bool> ReviewExist(int id)
        {
            return await _context.Reviews.AnyAsync(s => s.Id == id);
        }
    }
}