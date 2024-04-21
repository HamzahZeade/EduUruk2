using System.ComponentModel.DataAnnotations;

namespace EduUruk.Models.ViewModels
{
    public class ProfileForm
    {
        public string UserID { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceName = "EmailInvalid")]
        [StringLength(35, ErrorMessageResourceName = "EmailLength")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "UserName")]
        [Required(ErrorMessageResourceName = "UserNameRequired")]
        [StringLength(45, ErrorMessageResourceName = "UserNameLength")]
        public string UserName { get; set; }


        public ProfileForm()
        {

        }
    }
}
