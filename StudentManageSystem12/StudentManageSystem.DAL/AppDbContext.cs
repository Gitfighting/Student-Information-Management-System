using System.Data.Entity;
using StudentManageSystem.Model;

namespace StudentManageSystem.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=constr")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Score>()
                .HasRequired(s => s.Student)
                .WithMany()
                .HasForeignKey(s => s.stuId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
