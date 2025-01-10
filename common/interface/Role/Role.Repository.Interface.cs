

using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public interface IRoleRepository{
        Task<Role> add(Role role);
    }
}