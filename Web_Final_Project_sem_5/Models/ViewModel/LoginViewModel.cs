
using System.ComponentModel.DataAnnotations;

namespace Web_Final_Project_sem_5.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a role.")]
        public string Role { get; set; } = "Farmer";

        public bool RememberMe { get; set; }
    }
}
