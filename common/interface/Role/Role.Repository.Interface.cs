

using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public interface IRoleRepository{
        Task<Role> add(Role role);
        Task<Role> finByIdOrFail(int roleId);
        Task<Role?> GetUserRole(int userId);
    }
}