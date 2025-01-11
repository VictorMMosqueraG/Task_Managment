using TaskManagement.DTOs;
using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public interface IUserService{
        Task<List<object>> findAll(PaginationUserDto paginationUserDto);
    }
}