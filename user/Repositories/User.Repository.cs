
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
    }
}