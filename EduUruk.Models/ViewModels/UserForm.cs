using System.ComponentModel.DataAnnotations;

namespace EduUruk.Models.ViewModels
{
    public class UserForm
    {

        #region Properties 
        public string Id { get; set; }
        public string UserId { get; set; }
        public string? Createdby { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActivated { get; set; } = false;
        public Nullable<DateTime> ActivatedTime { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceName = "EmailInvalid")]
        [StringLength(100, ErrorMessageResourceName = "EmailLength")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Required")]
        public string[] RoleIds { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }

        public virtual List<SelectControl> Roles { get; set; }

        #endregion

        public UserForm()
        {
            Roles = new List<SelectControl>();
        }

    }
}
