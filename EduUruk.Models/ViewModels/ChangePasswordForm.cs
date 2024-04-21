using System.ComponentModel.DataAnnotations;

namespace EduUruk.Models.ViewModels
{
    public class ChangePasswordForm
    {
        [Required]
        [DataType(DataType.Password)]
        //[Display(Name = "Current password")]
        [Display(Name = "OldPassword")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        //[Display(Name = "New password")]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        //[Display(Name = "Confirm new password")]
        [Display(Name = "ConfirmPassword")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
