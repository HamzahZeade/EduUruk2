using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduUruk.Models.Auth_Tables
{
    [Table("User")]
    public class User : IdentityUser
    {
        public User() : base()
        {

        }

        #region Properties
        public string UserType { get; set; }
        public string Createdby { get; set; }
        public Nullable<DateTime> CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<DateTime> UpdatdTime { get; set; }
        public bool IsActivated { get; set; } = false;
        public string? Activatedby { get; set; }
        #endregion
        public string? TempPassword { get; set; }
        public bool? IsDeleted { get; set; }
        [NotMapped]
        public List<string> Roles { get; set; }
    }
}