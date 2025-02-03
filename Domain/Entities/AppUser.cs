using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        [MaxLength(50)] public required string RefreshToken { get; set; }
        public required DateTime RefreshTokenExpiryDate { get; set; }
        public string? PhotoUrl { get; set; }
        public required List<Review> Reviews { get; init; }
        public required List<Transaction> Transactions { get; init; }
    }
}