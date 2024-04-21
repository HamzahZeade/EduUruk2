using EduUruk.Models.Auth_Tables;
using EduUruk.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduUruk.DAL.EnitityDAL
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RolePermission>()
               .HasKey(c => new { c.PageID, c.RoleId });

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>()
                .HasIndex("Name")
                .IsUnique();
            //--------------------------------- to view :


        }

        public virtual DbSet<PageGroup> PagesGroups { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryLibrary> CategoryLibraries { get; set; }
        public DbSet<Comment> Comments { get; set; }



    }
}