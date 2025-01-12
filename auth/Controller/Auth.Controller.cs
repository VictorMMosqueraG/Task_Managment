
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TaskManagement.DTOs;
using TaskManagement.Interfaces;

namespace TaskManagement.Controllers{

    [Route("api/auth")]
    [ApiController]
    public class AuthController:ControllerBase{
        
        private readonly IAuthService service;

        public AuthController(IAuthService _service){
            service = _service;
        }

         /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="user">Objeto con la información del usuario para el registro.</param>
        /// <returns>Un código de estado 201 si el registro fue exitoso.</returns>
        /// <response code="201">Usuario registrado exitosamente.</response>
        /// <response code="400">Solicitud incorrecta o datos inválidos.</response>
        /// /// <response code="404">Role no encontrado.</response>
        /// <response code="500">Error no controlado .</response>
        [AllowAnonymous]
        [HttpPost("register")]
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
                throw new UnexpectedErrorException("Unexpected Error");
            }
        }


        /// <summary>
        /// Inicia sesión con las credenciales del usuario.
        /// </summary>
        /// <param name="loginDto">Objeto que contiene el correo electrónico y la contraseña.</param>
        /// <returns>Un token JWT si la autenticación es exitosa.</returns>
        /// <response code="200">Autenticación exitosa y token JWT.</response>
        /// <response code="401">Credenciales inválidas.</response>        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto){
            try
                {
                    var token = await service.loginUser(loginDto.Email, loginDto.Password);

                    return Ok(new{
                        status = 200,
                        message = "Auth successfully",
                        token 
                    });
            }catch (UnauthorizedAccessException){
                return Unauthorized(new{
                    status = 401,
                    message = "Credential invalid"
                });
            }   
        }
    }
}