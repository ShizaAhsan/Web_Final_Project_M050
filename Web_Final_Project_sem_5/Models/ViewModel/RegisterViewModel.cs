
using System.ComponentModel.DataAnnotations;

namespace Web_Final_Project_sem_5.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8,
            ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare(nameof(Password),
            ErrorMessage = "Password and Confirm Password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a role.")]
        [RegularExpression("^(Farmer|Admin)$",
            ErrorMessage = "Invalid role selected.")]
        public string Role { get; set; } = "Farmer";
    }
}
