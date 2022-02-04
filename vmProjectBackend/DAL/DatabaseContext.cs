using Microsoft.EntityFrameworkCore;
using vmProjectBackend.Models;



namespace vmProjectBackend.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSectionRole> UserSectionRoles { get; set; }
        public DbSet<VmTemplate> VmTemplates { get; set; }
        public DbSet<VmTemplateCourse> VmTemplateCourses { get; set; }
        public DbSet<VmInstance> vmUserInstances { get; set; }
        public object Configuration { get; }
    }
}