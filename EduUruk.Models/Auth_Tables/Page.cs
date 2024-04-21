using EduUruk.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduUruk.Models.Auth_Tables
{
    [Table("Page")]
    public class Page : _GlobalColumn3
    {
        #region Properties 
        [Key]
        [Display(Name = "PageID")]
        public int PageID { get; set; }

        [Display(Name = "GroupID")]
        public Nullable<int> GroupID { get; set; }

        [Display(Name = "ArabicPageName")]
        [Required(ErrorMessageResourceName = "ArabicPageNameRequired")]
        [StringLength(35, ErrorMessageResourceName = "ArabicPageNameLength")]
        public string ArabicPageName { get; set; }

        [Display(Name = "EnglishPageName")]
        [Required(ErrorMessageResourceName = "EnglishPageNameRequired")]
        [StringLength(35, ErrorMessageResourceName = "EnglishPageNameLength")]
        public string EnglishPageName { get; set; }

        [NotMapped]
        [Display(Name = "PageName")]
        public string PageName
        {
            get
            {
                return EnglishPageName;
            }
        }

        [Display(Name = "PageURL")]
        [Required(ErrorMessageResourceName = "PageURLRequired")]
        [StringLength(65, ErrorMessageResourceName = "PageURLLength")]
        public string PageURL { get; set; }

        [Display(Name = "GroupIco")]
        [StringLength(45, ErrorMessageResourceName = "GroupIcoLength")]
        public string? Ico { get; set; }


        [Display(Name = "PageIndex")]
        [Required(ErrorMessageResourceName = "PageIndexRequired")]
        public int? PageIndex { get; set; }
        public virtual PageGroup Group { get; set; }


        public virtual ICollection<RolePermission> Permissions { get; set; }

        #endregion

        public Page()
        {
            Permissions = new HashSet<RolePermission>();
        }
    }
}