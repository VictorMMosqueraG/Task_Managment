
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

          /// <summary>
        /// Obtiene una lista de usuarios con paginación.
        /// </summary>
        /// <param name="paginationUserDto">Datos de paginación para limitar y ordenar los resultados de usuarios.</param>
        /// <returns>Una lista de usuarios encontrados según los parámetros de paginación.</returns>
        /// <response code="200">Lista de usuarios.</response>
        /// <response code="401">No autorizado. Se requiere un token válido.</response>
        [Authorize(Policy = "ReadAllPolicy")]
        [HttpGet]
        public async Task<IActionResult> finAllUser([FromQuery] PaginationUserDto paginationUserDto){
            try{
                 var users = await service.findAll(paginationUserDto);

                return StatusCode(200, new {
                    status = 200,
                    data = users 
                });
            }
            catch (ArgumentException){
                throw new UnexpectedErrorException("Unexpected Error");
            }
           
        }


         /// <summary>
        /// Obtiene una lista completa de usuarios sin aplicar paginación.
        /// </summary>
        /// <returns>Una lista de todos los usuarios disponibles.</returns>
        /// <response code="200">Lista de usuarios obtenida exitosamente.</response>
        /// <response code="500">Error interno del servidor al intentar obtener los usuarios.</response> 
        [HttpGet("findUser")]
        public async Task<IActionResult> GetAllUsersAsync(){
            try{
                 var users = await service.GetAllUsersAsync();

                return StatusCode(200, new {
                    status = 200,
                    data = users 
                });
            }
            catch (ArgumentException){
                throw new UnexpectedErrorException("Unexpected Error");
            }
           
        }
    }
}