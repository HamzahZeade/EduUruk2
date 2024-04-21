using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduUruk.Models.Auth_Tables
{
    [Table("UserRole")]
    public class Role : IdentityRole
    {
        public string ArabicName { get; set; }

        [NotMapped]
        [Display(Name = "RoleName")]
        public string RoleName
        {
            get
            {
                return ArabicName;
            }
        }
        [NotMapped]
        public string? DefaultPage { get; set; }

        public virtual ICollection<RolePermission> Permissions { get; set; }

        public Role() : base()
        {
            Permissions = new HashSet<RolePermission>();
        }
        public Role(string name, string ar_name) : base(name)
        {
            this.ArabicName = ar_name;
            this.Name = name;
            this.NormalizedName = name;
            Permissions = new HashSet<RolePermission>();
        }
    }

}