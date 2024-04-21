using System.ComponentModel.DataAnnotations;

namespace EduUruk.Models.ViewModels
{
    public class ForgetPasswordForm
    {
        [Required(ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceName = "EmailInvalid")]
        [StringLength(35)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string ReCaptchaToken { get; set; }
    }
}
