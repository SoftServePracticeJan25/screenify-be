using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Account
{
    public class ChangePasswordDto : IValidatableObject
    {
        [Required(ErrorMessage = "Old password is required.")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required.")]
        [MinLength(12, ErrorMessage = "New password must be at least 12 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{12,}$",
            ErrorMessage = "New password must contain at least one lowercase letter, one uppercase letter, one number, and one special character.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("NewPassword", ErrorMessage = "Confirm password does not match new password.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // NewPassword != OldPassword validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OldPassword == NewPassword)
            {
                yield return new ValidationResult(
                    "New password must not be the same as the old password.",
                    new[] { nameof(NewPassword) });
            }
        }
    }

}
