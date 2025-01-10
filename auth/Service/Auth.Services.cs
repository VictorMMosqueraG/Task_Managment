
using TaskManagement.DTOs;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Services{

    public class AuthServices : IAuthService{
        private readonly IUserRepository repository;
        private readonly IRoleService roleService;

        private readonly IPermissionService permissionService;
        private readonly AuthServiceJwt authService;
        public AuthServices(
            IUserRepository _repository,
            IRoleService _service,
            AuthServiceJwt _authService,
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

        public async Task<string> loginUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            var user = await repository.FindByEmail(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new UnauthorizedAccessException("Credential Invalid."); // COMEBACK: Handle Error
            }

            var role = await roleService.GetUserRole(user.Id);
            if (role == null) throw new InvalidOperationException("Role not found.");

            var permissions = await permissionService.GetUserPermissions(user.Id);
            if (permissions == null) throw new InvalidOperationException("Permissions not found.");

            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            var token = authService.GenerateToken(user, role.Name, permissions.Select(p => p.Name).ToList());

            return token;
        }
    }
}