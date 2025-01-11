
using TaskManagement.DTOs;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Services{

    public class RoleService : IRoleService{

        private readonly IRoleRepository repository;
        private readonly IPermissionService permissionService;
        public RoleService(IRoleRepository _role, IPermissionService _permissionService){
            repository = _role;
            permissionService = _permissionService;
        }

        public async Task<Role> createRole(CreateRoleDTO dto){

            //Valid if the permission id provide is valid or not
            var permissions = new List<RolePermission>();

            foreach (var permissionId in dto.Permissions){
                var permission = await permissionService.findByIdOrFail(permissionId);
            }
            //Mapping data
             var role = new Role{
                Name = dto.Name,
                Description = dto.Description,
                Permissions = dto.Permissions.Select(permissionId => new RolePermission
                {
                    PermissionId = permissionId,
                }).ToList()
            };
            
            return await repository.add(role);
        }

        //NOTE: FindByIdOrFail if not exist throw Exception
        public async Task<Role> findByIdOrFail(int roleId){
            var role = await repository.finByIdOrFail(roleId);

            return role;
        }
        
        //NOTE: Find a user in the database by their ID, including the relationship with their role  
        
        public async Task<Role?> GetUserRole(int userId){
            var role = await repository.GetUserRole(userId);
            return role;
        }
        
    }
}