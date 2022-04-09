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

        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<SessionToken> SessionTokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSectionRole> UserSectionRoles { get; set; }
        public DbSet<VmDetail> VmDetails { get; set; }
        public DbSet<VmTable> VmTables { get; set; }
        public DbSet<VmTemplate> VmTemplates { get; set; }
        public DbSet<VmUtilization> VmUtilizations { get; set; }
        public object Configuration { get; }
    }
}