using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        public async Task<string> UploadAvatarAsync(IFormFile file, ClaimsPrincipal user)
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

            var fileName = $"{userId}{fileExtension}";

            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainer.GetBlobClient(fileName);

            // Delete file, if such already exists
            await blobClient.DeleteIfExistsAsync();

            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });

            var fileUrl = blobClient.Uri.ToString();

            // Update PhotoUrl in DB
            var userEntity = await _userManager.FindByIdAsync(userId);
            if (userEntity != null)
            {
                userEntity.PhotoUrl = fileUrl;
                await _userManager.UpdateAsync(userEntity);
            }

            return fileUrl;
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
