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
using AutoMapper;

namespace Infrastructure.Services
{
    public class ReviewService : IReviewService
{
    private readonly MovieDbContext _context;
    private readonly IMapper _mapper;

    public ReviewService(MovieDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReviewReadDto> AddAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
        return _mapper.Map<ReviewReadDto>(review);
    }

    public async Task<List<ReviewReadDto>> GetAllAsync()
    {
        var reviews = await _context.Reviews
        .Include(r => r.AppUser) 
        .ToListAsync();
    
    return reviews.Select(r => _mapper.Map<ReviewReadDto>(r)).ToList();
    }

    public async Task<ReviewReadDto?> GetByIdAsync(int id)
    {
        var review = await _context.Reviews
        .Include(r => r.AppUser) 
        .FirstOrDefaultAsync(x => x.Id == id);

        return review == null ? null : _mapper.Map<ReviewReadDto>(review);
    }

    public async Task<ReviewReadDto?> UpdateAsync(int id, ReviewUpdateDto reviewUpdateDto)
{
    var existingReview = await _context.Reviews
        .Include(r => r.AppUser) 
        .FirstOrDefaultAsync(x => x.Id == id);

    if (existingReview == null)
    {
        return null;
    }

    existingReview.Rating = reviewUpdateDto.Rating;
    existingReview.Comment = reviewUpdateDto.Comment;
    existingReview.Likes = reviewUpdateDto.Likes;

    await _context.SaveChangesAsync();

    return _mapper.Map<ReviewReadDto>(existingReview);
}


    public async Task<ReviewReadDto?> DeleteAsync(int id)
    {
        var reviewModel = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);

        if (reviewModel == null)
        {
            return null;
        }

        _context.Reviews.Remove(reviewModel);
        await _context.SaveChangesAsync();
        return _mapper.Map<ReviewReadDto>(reviewModel);
    }

    public async Task<bool> ReviewExist(int id)
    {
        return await _context.Reviews.AnyAsync(s => s.Id == id);
    }
}

}