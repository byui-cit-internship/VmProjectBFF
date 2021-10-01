using Microsoft.EntityFrameworkCore;
using vmProjectBackend.Models;
// using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;



namespace vmProjectBackend.DAL
{
    public class VmContext :DbContext
    {
        // public VmContext(): base("VmContext")
        // {
        // }
        public VmContext(DbContextOptions<VmContext> options)
           : base(options)
        {
            
        }

        public DbSet<User> Users {get;set;}
        public DbSet<Enrollment> Enrollments {get;set;}
        public DbSet<Course> Courses {get;set;}
        public DbSet<VmTable> VmTables {get;set;}

        // protected override void OnModelCreating(DbModelBuilder modelBuilder)
        // {
        //     modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        // }


    }
}