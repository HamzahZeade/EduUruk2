using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduUruk.Models.Auth_Tables
{
    [Table("PageGroup")]
    public class PageGroup
    {
        #region Properties 
        [Key]
        public int GroupID { get; set; }

        [Required(ErrorMessageResourceName = "ArabicGroupNameRequired")]
        [StringLength(35, ErrorMessageResourceName = "ArabicGroupNameLength")]
        public string ArabicGroupName { get; set; }

        [Required(ErrorMessageResourceName = "EnglishGroupNameRequired")]
        [StringLength(65, ErrorMessageResourceName = "EnglishGroupNameLength")]
        public string EnglishGroupName { get; set; }

        [NotMapped]
        public string GroupName
        {
            get
            {
                return EnglishGroupName;
            }
        }

        [Required(ErrorMessageResourceName = "GroupIcoRequired")]
        [StringLength(45, ErrorMessageResourceName = "GroupIcoLength")]
        public string? GroupIco { get; set; }


        [Required(ErrorMessageResourceName = "PageGroupIndexRequired")]
        //[StringLength(45, ErrorMessageResourceName = "GroupIcoLength", ErrorMessageResourceType = typeof(Resources.SysModelRes))]
        public int PageGroupIndex { get; set; }
        public int? GroupOrder { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        #endregion
        public PageGroup()
        {
            Pages = new HashSet<Page>();
        }
    }
}