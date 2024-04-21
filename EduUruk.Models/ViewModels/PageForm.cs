using System.ComponentModel.DataAnnotations;

namespace EduUruk.Models.ViewModels
{
    public class PageForm
    {
        #region Properties 

        public int PageID { get; set; }
        public Nullable<int> GroupID { get; set; }
        [Required(ErrorMessageResourceName = "Required")]
        [StringLength(35, ErrorMessageResourceName = "Required_Length")]
        public string ArabicPageName { get; set; }
        [Required(ErrorMessageResourceName = "Required")]
        [StringLength(35, ErrorMessageResourceName = "Required_Length")]
        public string EnglishPageName { get; set; }
        [Required(ErrorMessageResourceName = "Required")]
        [StringLength(65, ErrorMessageResourceName = "Required_Length")]
        public string PageURL { get; set; }
        [Required(ErrorMessageResourceName = "Required")]
        public Nullable<int> PageIndex { get; set; }
        [Display(Name = "GroupIco")]
        public string? Ico { get; set; }

        public virtual List<SelectControl> Groups { get; set; }
        #endregion

        public PageForm()
        {
            Groups = new List<SelectControl>();
        }
    }
}