using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Account
{
    public class LoginDto
    {
        [Required]
        public required string Username { get; init; }

        [Required]
        public required string Password { get; init; }
    }
}
