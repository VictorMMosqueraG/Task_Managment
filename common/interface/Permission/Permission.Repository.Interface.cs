using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public interface IPermissionRepository{
        Task<Permission> add(Permission permission);
    }
}