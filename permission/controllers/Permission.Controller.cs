using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TaskManagement.DTOs;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Controllers{

    [Route("api/permission")]
    [ApiController]
    public class PermissionController: ControllerBase{
        private readonly IPermissionService _service;

        public PermissionController(IPermissionService service){
            _service = service;
        }

        //NOTE: Save Permission
        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionDto permission){
            try{
                var createdPermission = await _service.createPermission(permission);
                
                return StatusCode(201, new {
                    status = 201,
                    message = "Entity was created Successfully." 
                }); 
            }catch(DbUpdateException){
                throw new AlreadyExistException("NAME");
            }
            catch (ArgumentException){
                throw new UnexpectedErrorException();
            }
        }
    }
    
}