using TaskManagement.DTOs;
using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public interface IRoleService{
        Task<Role> createRole(CreateRoleDTO role);
        Task<Role> findByIdOrFail(int roleId);

        Task<Role?> GetUserRole(int userId);
    }
}