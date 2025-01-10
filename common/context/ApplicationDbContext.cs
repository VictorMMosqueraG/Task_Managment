using Microsoft.EntityFrameworkCore;  
using TaskManagement.Entity  ;
using TaskManagement.Entity.Configuration;

namespace TaskManagement.Data 
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        //NOTE: DbSet for my entities
        //NOTE: The `null!` operator prevents the compiler from throwing an error for an uninitialized property
        public DbSet<Permission> Permissions { get; set; } = null!; 
        public DbSet<Role> Role {get;set;} = null!; 
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;

       //NOTE: Apply configuration of the entities
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            //NOTE: Permission
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());

            //NOTE: Roles
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            //NOTE: Role_Permission
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
        }
    }   
}
