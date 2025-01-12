using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.DTOs;
using TaskManagement.Interfaces;

namespace TaskManagement.Controllers{



    [Route("api/permission")]
    [ApiController]
    public class PermissionController: ControllerBase{
        private readonly IPermissionService _service;

        public PermissionController(IPermissionService service){
            _service = service;
        }

        /// <summary>
        /// Crea un nuevo permiso en el sistema.
        /// </summary>
        /// <param name="permission">Objeto con la información del permiso a crear.</param>
        /// <returns>Un código de estado 201 si el permiso se creó exitosamente.</returns>
        /// <response code="201">Permiso creado exitosamente.</response>
        /// <response code="400">Nombre ya registrado en la db.</response>
        ///  /// <response code="401">No autorizado. Se requiere un token válido.</response>
        //NOTE: Save Permission
        [Authorize(Policy = "WriteAllPolicy")]
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
                throw new UnexpectedErrorException("Unexpected Error");
            }
        }
    }
    
}