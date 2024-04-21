using System.ComponentModel.DataAnnotations;
namespace EduUruk.Models.ViewModels
{
    public class ResetPasswordForm
    {
        #region Properties

        public string userId { get; set; }

        public string Token
        {
            get;
            set;
        }
        //------------------------------------------

        public bool IsValid
        {
            get;
            set;
        }
        //------------------------------------------


        [Display(Name = "NewPassword")]
        [Required(ErrorMessageResourceName = "NewPasswordRequired")]
        [DataType(DataType.Password)]
        [StringLength(16, ErrorMessageResourceName = "NewPasswordExceed")]
        /// <summary>
        /// Gets or sets  NewPassword. 
        /// </summary>
        public string NewPassword
        {
            get;
            set;
        }
        //------------------------------------------

        [Display(Name = "ConfirmPassword")]
        [Required(ErrorMessageResourceName = "ConfirmPasswordRequired")]
        [DataType(DataType.Password)]
        [StringLength(16, ErrorMessageResourceName = "ConfirmPasswordExceed")]
        [Compare("NewPassword", ErrorMessageResourceName = "ConfirmPasswordCompare")]
        public string ConfirmPassword
        {
            get;
            set;
        }
        //------------------------------------------

        #endregion
    }
}
