using TaskManagement.DTOs;
using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public interface IUserService{
        Task<User> createUser(CreateUserDto dto);
        Task<String> loginUser(string email, string password);
    }
}