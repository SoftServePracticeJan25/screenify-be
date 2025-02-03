using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Account
{
    public class RegisterDto
    {
        [Required]
        public required string Username { get; init; }
        [Required]
        [EmailAddress]
        public required string Email { get; init; }
        [Required]
        public required string Password { get; init; }
        public string? PhotoUrl { get; init; }
    }
}
