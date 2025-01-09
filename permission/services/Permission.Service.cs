using System.ComponentModel.DataAnnotations;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Services{

    public class PermissionService : IPermissionService{

        private readonly IPermissionRepository repository;

        public PermissionService(IPermissionRepository _repository){
            repository = _repository;
        }

        public async Task<Permission> createPermission(Permission permission){
            return await repository.add(permission);
        }
    }
}