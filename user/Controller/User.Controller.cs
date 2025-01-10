
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.DTOs;
using TaskManagement.Interfaces;

namespace TaskManagement.Controllers{

    [Route("api/auth")]
    [ApiController]
    public class UserController:ControllerBase{

        private readonly IUserService service;

        public UserController(IUserService _service){
            service = _service;
        }

        public async Task<IActionResult> createUser([FromBody] CreateUserDto user){
            try{
                var createUser = await service.createUser(user);
                
                return StatusCode(201, new {
                    status = 201,
                    message = "Entity was created successfully"
                });
            }
            catch(DbUpdateException){
                throw new AlreadyExistException("Email");
            }
            catch (ArgumentException){
                throw new UnexpectedErrorException();
            }
        }
    }
}