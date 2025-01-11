
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.DTOs;
using TaskManagement.Interfaces;

namespace TaskManagement.Controllers{

    [Route("api/user")]
    [ApiController]
    public class UserController:ControllerBase{

        private readonly IUserService service;

        public UserController(IUserService _service){
            service = _service;
        }

        [Authorize(Policy = "ReadAllPolicy")]
        [HttpGet]
        public async Task<IActionResult> finAllUser([FromQuery] PaginationUserDto paginationUserDto){
            var users = await service.findAll(paginationUserDto);

            return StatusCode(200, new {
                    status = 200,
                    data = users 
            });
        }
    }
}