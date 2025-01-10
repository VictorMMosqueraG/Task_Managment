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
    }
}