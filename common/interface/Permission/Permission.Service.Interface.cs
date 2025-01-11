using TaskManagement.DTOs;
using TaskManagement.Entity;

namespace TaskManagement.Interfaces{
    
    public interface IPermissionService{
        Task<Permission> createPermission(CreatePermissionDto permission);
        Task<Permission?> findByIdOrFail(int permissionId);
         Task<List<Permission?>?> GetUserPermissions(int userId);
    }
}