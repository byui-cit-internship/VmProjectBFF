using Microsoft.EntityFrameworkCore;
using vmProjectBackend.Models;
// using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;



namespace vmProjectBackend.DAL
{
    public class VmContext : DbContext
    {
        public VmContext(DbContextOptions<VmContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<VmTable> VmTables { get; set; }

        public DbSet<Token> Tokens { get; set; }
        // public DbSet<VmTableCourse> VmTableCourse { get; set; }
        public object Configuration { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Token>().ToTable("Token");
            modelBuilder.Entity<VmTable>().ToTable("VmTable");
            // modelBuilder.Entity<VmTableCourse>().ToTable("VmTableCourse");
        }

    }
}