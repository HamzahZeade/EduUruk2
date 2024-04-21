using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduUruk.Models.Auth_Tables
{
    [Table("RolePermission")]
    public class RolePermission
    {
        #region Properties 
        [Key, Column(Order = 0)]
        public string RoleId { get; set; }

        [Key, Column(Order = 1)]
        [Display(Name = "PageID")]
        public int PageID { get; set; }

        [Display(Name = "CanSelect")]
        public bool CanSelect { get; set; }

        [Display(Name = "CanCreate")]
        public bool CanCreate { get; set; }

        [Display(Name = "CanEdit")]
        public bool CanEdit { get; set; }

        [Display(Name = "CanDelete")]
        public bool CanDelete { get; set; }

        [Display(Name = "CanActive")]
        public bool CanActive { get; set; }

        [Display(Name = "CanPrint")]
        public bool CanPrint { get; set; }

        public virtual Page Page { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("Permissions")]
        public virtual Role UserRole { get; set; }
        #endregion

        public RolePermission()
        {

        }

    }

}