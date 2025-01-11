using Microsoft.EntityFrameworkCore;
using Npgsql;
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

        //NOTE: FindByIdOrFail
        public async Task<Permission?> findByIdOrFail(int permissionId){
            var permission = await context.Permissions
                .FirstOrDefaultAsync(p => p.Id == permissionId);

            if (permission == null){
                throw new NotFoundException(permissionId,"PERMISSION");
            }

            return permission;
        }

        //NOTE: Method to get the list of permissions for a specific user by their user ID
        public async Task<List<Permission?>?> GetUserPermissions(int userId){
            var user = await context.User
                .Include(u => u.role)
                .ThenInclude(r => r.Permissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.role.Permissions.Select(rp => rp.Permission).ToList();
        }
    }
}