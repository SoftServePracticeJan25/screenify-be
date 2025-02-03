using System.ComponentModel.DataAnnotations;
namespace Domain.DTOs.Account
{
    public class UserInfoDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; init; }
        [Required]
        public required string PhotoUrl { get; init;}
        [Required]
        public required string Username { get; init; }
        [Required]
        public required string Id { get; init; }
        [Required]
        public required int ReviewCount { get; init; }
        [Required]
        public required int TransactionCount { get; init; }
    }
}