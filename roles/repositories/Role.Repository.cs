using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public class RoleRepository : IRoleRepository{

        private readonly ApplicationDbContext context;

        public RoleRepository(ApplicationDbContext _context){
            context = _context;
        }
        public async Task<Role> add(Role role){
            context.Role.Add(role);
            await context.SaveChangesAsync();
            return role;
        }

        //NOTE: FIndByIdOrFail
        public async Task<Role> finByIdOrFail(int roleId){
            var role = await context.Role
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if(role == null){
                throw new NotFoundException(roleId,"ROLE");
            }

            return role;
        }

        public async Task<Role?> GetUserRole(int userId){
            var user = await context.User
            .Include(u => u.role)
            .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.role;
        }

    }
}