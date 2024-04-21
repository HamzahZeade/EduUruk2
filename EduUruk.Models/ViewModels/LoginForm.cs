using System.ComponentModel.DataAnnotations;

namespace EduUruk.Models.ViewModels
{
    public class LoginForm
    {
        [Required(ErrorMessageResourceName = "UserNameRequired")]
        [StringLength(maximumLength: 100, MinimumLength = 6, ErrorMessageResourceName = "Required_Length")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
        public string? Active { get; set; }


    }
    public class LoginForm_ChangePass
    {

        public string UserName { get; set; }

        [Display(Name = "Password")]
        [StringLength(maximumLength: 20, MinimumLength = 6)]
        [Required(ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "Required")]
        [StringLength(maximumLength: 20, MinimumLength = 6, ErrorMessageResourceName = "PasswordLength")]
        [Compare("Password", ErrorMessageResourceName = "ConfirmPasswordCompare")]
        [Display(Name = "ConfirmPassword")]
        public string? ConfirmPassword { get; set; }
        public string? ReturnUrl { get; set; }
        public string TempPassword { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginForm_OTP
    {

        public string? Mobile { get; set; }

        public string? ReturnUrl { get; set; }
        public long? sub_id { get; set; }
        public string? OTP { get; set; }

        public bool RememberMe { get; set; }
    }

}
