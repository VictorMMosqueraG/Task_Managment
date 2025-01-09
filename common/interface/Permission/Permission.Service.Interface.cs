using TaskManagement.Entity;

namespace TaskManagement.Interfaces{
    
    public interface IPermissionService{
        Task<Permission> createPermission(Permission permission);
    }
}