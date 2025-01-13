using TaskManagement.DTOs;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Services{

    public class UserService : IUserService{
        
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository){
            this.userRepository = userRepository;
        }

        public async Task<List<object>> findAll(PaginationUserDto paginationUserDto){
            //Find all users
            var findUser = await userRepository.findAll();

            //If search email is not null, filter the users
            var filteredUsers = findUser.Where(
                    user => user.Email.Contains(paginationUserDto.Email)
                ).ToList();
            
            //Sorting the users
            var sortedUsers = paginationUserDto.OrderBy.ToLower() == "asc"
                ? filteredUsers.OrderBy(user => user.Email)
                : filteredUsers.OrderByDescending(user => user.Email);

            //Pagination: Skip the offset and take the limit and mapping data
            var pagedUsers = sortedUsers
                .Skip(paginationUserDto.Offset)
                .Take(paginationUserDto.Limit)
                .Select(user => new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.role.Name
                }).ToList();
            

            return  pagedUsers.Cast<object>().ToList();  
        }

        public async Task<User?> findByIdOrFail(int userId){
            var user = await userRepository.findByIdOrFail(userId);

            return user;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await userRepository.GetAllUsersAsync();
        }
    }

}