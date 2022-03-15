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
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMembership> GroupMemberships { get; set; }
        public DbSet<IpAddress> IpAddresses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<SessionToken> SessionTokens { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagCategory> TagCategories { get; set; }
        public DbSet<TagUser> TagUser { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSectionRole> UserSectionRoles { get; set; }
        public DbSet<Vlan> Vlans { get; set; }
        public DbSet<VlanVswitch> VlanVswitches { get; set; }
        public DbSet<VmInstance> VmInstances { get; set; }
        public DbSet<VmInstanceIpAddress> VmInstanceIpAddresses { get; set; }
        public DbSet<VmInstanceTag> VmInstanceTags { get; set; }
        public DbSet<VmInstanceVswitch> VmInstanceVswitches { get; set; }
        public DbSet<VmTemplate> VmTemplates { get; set; }
        public DbSet<VmTemplateTag> VmTemplateTags { get; set; }
        public DbSet<Vswitch> Vswitchs { get; set; }
        public DbSet<VswitchTag> VswitchTags { get; set; }
        public object Configuration { get; }
    }
}