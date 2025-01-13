using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public interface IUserRepository{
        Task<User> add(User user);
        Task<User?> FindByEmail(string email);
        Task<List<User>> findAll();

        Task<User?> findByIdOrFail(int userId);
        Task<List<User>> GetAllUsersAsync();//Method by forms view
    }
}