using TaskManagement.DTOs;
using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public interface IRoleService{
        Task<Role> createRole(CreateRoleDTO role);
    }
}