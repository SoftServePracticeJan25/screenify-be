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
    public class UserInfoService(UserManager<AppUser> userManager) : IUserInfoService
    {
        public async Task<UserInfoDto> GetUserInfo(AppUser appUser)
        {
            var userId = appUser.Id;

            var user = await userManager.Users
            .Include(u => u.Reviews)  
            .Include(u => u.Transactions)  
            .FirstOrDefaultAsync(u => u.Id == userId);

            var roles = (await userManager.GetRolesAsync(user!)).ToList();

            if (user == null)
                throw new Exception("User not found");
            
            return new UserInfoDto
            {
                Id = user.Id,
                Email = user.Email!,
                Username = user.UserName!,
                PhotoUrl = user.PhotoUrl!, 
                ReviewCount = user.Reviews.Count, 
                TransactionCount = user.Transactions.Count,
                Role = roles,
                EmailConfirmed = user.EmailConfirmed
            };
        }
        public async Task<IEnumerable<UserInfoDto>> GetAllUsersAsync()
        {
            var users = await userManager.Users
                .Include(u => u.Reviews)
                .Include(u => u.Transactions)
                .ToListAsync();

            var userDtos = new List<UserInfoDto>();

            foreach (var user in users)
            {
                var roles = (await userManager.GetRolesAsync(user)).ToList();
                userDtos.Add(new UserInfoDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Username = user.UserName!,
                    PhotoUrl = user.PhotoUrl!,
                    ReviewCount = user.Reviews.Count,
                    TransactionCount = user.Transactions.Count,
                    Role = roles,
                    EmailConfirmed = user.EmailConfirmed
                });
            }

            return userDtos;
        }
    }
}
