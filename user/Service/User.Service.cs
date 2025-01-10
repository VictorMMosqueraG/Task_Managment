using TaskManagement.DTOs;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Services{

    public class UserService : IUserService{

        private readonly IUserRepository repository;
        private readonly IRoleService roleService;

        private readonly IPermissionService permissionService;
        private readonly AuthService authService;
        public UserService(
            IUserRepository _repository,
            IRoleService _service,
            AuthService _authService,
            IPermissionService _permissionService

        ){
            repository = _repository;
            roleService = _service;
            authService = _authService;
            permissionService = _permissionService;
        }
        
        public async Task<User> createUser(CreateUserDto dto){

            //Valid if the role if provide is valid or not
            var role = await roleService.findByIdOrFail(dto.role);

            // Encrypt the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.password);
            //Mapping data
            var user = new User{
                Name = dto.name,
                Email = dto.email,
                Password = hashedPassword,
                role = role
            };

            return await repository.add(user);
        }

        public async Task<String> loginUser(string email, string password){
            var user = await repository.FindByEmail(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password)){
                throw new UnauthorizedAccessException("Credential Invalid."); //COMEBACK: Handle Error
            }

            var role = await roleService.GetUserRole(user.Id);
            var permissions = await permissionService.GetUserPermissions(user.Id);

            var token = authService.GenerateToken(user, role.Name, permissions.Select(p => p.Name).ToList());

            return token;
    
        }
    }

}