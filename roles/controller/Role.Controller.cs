using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.DTOs;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Controllers{

    [Route("api/role")]
    [ApiController]
    public class RoleController: ControllerBase{

        private readonly IRoleService service;
       

        public RoleController(IRoleService _service){
            service = _service;
        }

        //NOTE: Save Role
        [Authorize(Policy = "WriteAllPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO role){
            try{
                var createRole = await service.createRole(role);

                return StatusCode(201, new {
                    status = 201,
                    message = "Entity was created Successfully." 
                });            
            }catch(DbUpdateException){
                throw new AlreadyExistException("NAME");
            }
            catch (ArgumentException){
                throw new UnexpectedErrorException("Unexpected Error");
            }
        }
    }
}