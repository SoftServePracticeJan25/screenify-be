using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.DTOs.Account;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class AvatarService : IAvatarService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly UserManager<AppUser> _userManager;
        private readonly string _containerName = "avatars";
        private readonly long _maxFileSize = 2 * 1024 * 1024; // 2MB
        private readonly List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };

        public AvatarService(BlobServiceClient blobServiceClient, UserManager<AppUser> userManager)
        {
            _blobServiceClient = blobServiceClient;
            _userManager = userManager;
        }

        public async Task<UserInfoDto> UploadAvatarAsync(IFormFile file, ClaimsPrincipal user)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            if (file.Length > _maxFileSize)
                throw new ArgumentException("File is too large. Max size is 2MB.");

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!_allowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Invalid file type. Allowed: .jpg, .jpeg, .png");

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Invalid token. No user ID found.");

            //var fileName = $"{userId}{fileExtension}";
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainer.GetBlobClient(fileName);

            // Delete file, if such already exists
            await blobClient.DeleteIfExistsAsync();

            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

            var fileUrl = blobClient.Uri.ToString();

            // Update PhotoUrl in DB
            var userEntity = await _userManager.FindByIdAsync(userId);
            if (userEntity == null)
                throw new Exception("User not found");

            if (!string.IsNullOrEmpty(userEntity.PhotoUrl))
            {
                var oldBlobName = Path.GetFileName(new Uri(userEntity.PhotoUrl).LocalPath);
                var oldBlobClient = _blobServiceClient.GetBlobContainerClient(_containerName).GetBlobClient(oldBlobName);
                await oldBlobClient.DeleteIfExistsAsync();
            }

            userEntity.PhotoUrl = fileUrl;
            await _userManager.UpdateAsync(userEntity);

            userEntity = await _userManager.Users
            .Include(u => u.Reviews)
            .Include(u => u.Transactions)
            .FirstOrDefaultAsync(u => u.Id == userId);

            // Getting user's role
            var role = (await _userManager.GetRolesAsync(userEntity)).FirstOrDefault();

            return new UserInfoDto
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                Username = userEntity.UserName,
                PhotoUrl = userEntity.PhotoUrl,
                ReviewCount = userEntity.Reviews.Count,
                TransactionCount = userEntity.Transactions.Count,
                Role = role,
                EmailConfirmed = userEntity.EmailConfirmed
            };
        }


        public async Task<string?> GetAvatarUrlAsync(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return null;

            var userEntity = await _userManager.FindByIdAsync(userId);
            return userEntity?.PhotoUrl;
        }
    }


}
