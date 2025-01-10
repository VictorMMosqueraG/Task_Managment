using TaskManagement.DTOs;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Services{

    public class UserService : IUserService{

        private readonly IUserRepository repository;
        private readonly IRoleService roleService;

        public UserService(IUserRepository _repository, IRoleService _service){
            repository = _repository;
            roleService = _service;
        }
        public async Task<User> createUser(CreateUserDto dto){

            //Valid if the role if provide is valid or not
            var role = await roleService.findByIdOrFail(dto.role);

            //Mapping data
            var user = new User{
                Name = dto.name,
                Email = dto.email,
                Password = dto.password,
                role = role
            };

            return await repository.add(user);
        }
    }

}