using System.ComponentModel.DataAnnotations;

namespace EduUruk.Models.ViewModels
{
    public class RegisterForm
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Username must be between 6 and 100 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        // Add more properties as needed for registration (e.g., email, full name, etc.)
    }
}
