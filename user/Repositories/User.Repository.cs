
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Repositories{

    public class UserRepository : IUserRepository{
        
        private readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext _context){
            context = _context;
        }

        public async Task<User> add(User user){
            context.User.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> FindByEmail(string email){
            return await context.User
            .Include(u => u.role)
            .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> findAll(){
             var users = await context.User
                    .Include(u => u.role) // Include related role
                    .ToListAsync();

            return users;
        }

        public async Task<User?> findByIdOrFail(int userId){
            var user = await context.User
                .Include(u => u.role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null){
                throw new NotFoundException(userId,"USER");
            }

            return user;
        }
    }
}