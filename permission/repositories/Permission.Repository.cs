using TaskManagement.Data;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Repositories{

    public class PermissionRepository : IPermissionRepository{
        private readonly ApplicationDbContext context;

        public PermissionRepository(ApplicationDbContext _context){
            context = _context;
        }

        //NOTE: Save Permission
        public async Task<Permission> add(Permission permission){
            context.Permissions.Add(permission);
            await context.SaveChangesAsync();
            return permission;
        }
    }
}