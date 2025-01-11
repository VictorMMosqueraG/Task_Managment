using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public interface IPermissionRepository{
        Task<Permission> add(Permission permission);
        Task<Permission?> findByIdOrFail(int permissionId);
        Task<List<Permission?>?> GetUserPermissions(int userId);
    }
}