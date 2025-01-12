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

        /// <summary>
        /// Crea un nuevo rol en el sistema.
        /// </summary>
        /// <param name="role">Objeto con la información del rol a crear.</param>
        /// <returns>Un código de estado 201 si el rol se creó exitosamente.</returns>
        /// <response code="201">Rol creado exitosamente.</response>
        /// <response code="400">EL role ya existe o el id del permiso no se encontro.</response>
        /// <response code="401">No autorizado. Se requiere un token válido.</response>
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