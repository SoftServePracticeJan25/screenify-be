using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs.Account;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Extentions;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfoService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserInfoDto> GetUserInfo(AppUser appUser)
        {
            var userId = appUser.Id;

            var user = await _userManager.Users
            .Include(u => u.Reviews)  
            .Include(u => u.Transactions)  
            .FirstOrDefaultAsync(u => u.Id == userId);

            var roles = (await _userManager.GetRolesAsync(user)).ToList();

            if (user == null)
                throw new Exception("User not found");
            
            return new UserInfoDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                PhotoUrl = user.PhotoUrl, 
                ReviewCount = user.Reviews.Count, 
                TransactionCount = user.Transactions.Count,
                Role = roles,
                EmailConfirmed = user.EmailConfirmed
            };
        }
    }
}
