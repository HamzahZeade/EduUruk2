using EduUruk.Models.Auth_Tables;
using System.ComponentModel.DataAnnotations;

namespace EduUruk.Models.ViewModels
{
    public class RoleForm
    {
        #region Properties 
        public string RoleID { get; set; }
        [Display(Name = "ArabicGroupName")]
        [Required(ErrorMessageResourceName = "Required")]
        [StringLength(65, ErrorMessageResourceName = "Required_Length")]
        public string ArabicRoleName { get; set; }
        [Required(ErrorMessageResourceName = "Required")]
        [StringLength(65, ErrorMessageResourceName = "Required_Length")]
        public string EnglishRoleName { get; set; }
        public string? DefaultPage { get; set; }
        public List<RolePermission> Permissions { get; set; }
        #endregion
        public RoleForm()
        {
            Permissions = new List<RolePermission>();
        }
    }
}