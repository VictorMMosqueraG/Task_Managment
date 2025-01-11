using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public interface IUserRepository{
        Task<User> add(User user);
        Task<User?> FindByEmail(string email);
    }
}