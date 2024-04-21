using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduUruk.Models.Entities
{
    public class _GlobalColumn : _GlobalColumn2
    {
        [Display(Name = "مفعل ؟")]
        public bool? IsActive { get; set; }
        [Display(Name = "CreatedOn")]
        public DateTime? CreatedOn { get; set; } = DateTime.Now;
        [Display(Name = "CreatedBy")]
        public string CreatedBy { get; set; }
        [Display(Name = "ChangedOn")]
        public DateTime? ChangedOn { get; set; } = DateTime.Now;
        [Display(Name = "ChangedBy")]
        public string? ChangedBy { get; set; }


    }
    public class _GlobalColumn2 : _GlobalColumn3
    {
        public bool? IsDeleted { get; set; } = false;
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
    }
    public class _GlobalColumn3
    {
        [NotMapped]
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
    }
}