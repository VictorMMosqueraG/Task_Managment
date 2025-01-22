
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.DTOs;
using TaskManagement.Interfaces;

namespace TaskManagement.Controllers{

    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase{
        private readonly ITaskService taskService;
        
        public TaskController(
            ITaskService _taskService
        ){
            taskService = _taskService;
        }


        /// <summary>
        /// Crea una nueva tarea.
        /// </summary>
        /// <param name="dto">Objeto con los datos necesarios para crear una tarea.</param>
        /// <returns>Un código de estado 201 si la tarea fue creada exitosamente.</returns>
        /// <response code="201">Tarea creada exitosamente.</response>
        /// <response code="400">Solicitud incorrecta o datos inválidos.</response>
        /// /// <response code="404">No se encontro el user id.</response>
        /// <response code="401">No autorizado. Se requiere un token válido.</response>
        [Authorize(Policy = "WriteOrWriteAllPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody]CreateTaskDto dto){
            try{
                var createTasks = await taskService.CreateTask(dto);

                return StatusCode(201, new {
                    status = 201,
                    message = "Entity was created Successfully."
                });
            }
            catch (ArgumentException){
                throw new UnexpectedErrorException("Unexpected Error");
            }
        }

        /// <summary>
        /// Obtiene todas las tareas con paginación.
        /// </summary>
        /// <param name="paginationTaskDto">Datos de paginación para limitar y ordenar los resultados.</param>
        /// <returns>Una lista de tareas encontradas según los parámetros de paginación.</returns>
        /// <response code="200">Lista de tareas.</response>
        /// <response code="401">No autorizado. Se requiere un token válido.</response>        
        [Authorize(Policy = "ReadOrReadAllPolicy")]
        [HttpGet]
        public async Task<IActionResult> findAllTask(
            [FromQuery] PaginationTaskDto paginationTaskDto
        ){
            try{
                var foundTasks = await taskService.findAll(paginationTaskDto);    
               return Ok(foundTasks);
            }
            catch (ArgumentException){
                throw new UnexpectedErrorException("Unexpected Error");
            }
        }


        /// <summary>
        /// Elimina una tarea por su ID.
        /// </summary>
        /// <param name="id">ID de la tarea que se desea eliminar.</param>
        /// <returns>Un código de estado 200 si la tarea fue eliminada exitosamente.</returns>
        /// <response code="200">Tarea eliminada exitosamente.</response>
        /// <response code="404">El id no se encontro.</response>
        /// <response code="401">No autorizado. Se requiere un token válido.</response>        
        [Authorize(Policy = "DeleteOrDeleteAllPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteTask(
            [FromRoute] int id
        ){
            try{
                var foundTask = await taskService.delete(id);
                return Ok(foundTask);
            }
            catch (ArgumentException){
                throw new UnexpectedErrorException("Unexpected Error");
            }

        }


         /// <summary>
        /// Actualiza una tarea existente por su ID.
        /// </summary>
        /// <param name="updateTaskDto">Objeto con los datos a actualizar en la tarea.</param>
        /// <param name="id">ID de la tarea a actualizar.</param>
        /// <returns>Un código de estado 201 si la tarea fue actualizada exitosamente.</returns>
        /// <response code="201">Tarea actualizada exitosamente.</response>
        /// <response code="400">Solicitud incorrecta o datos inválidos.</response>
        /// /// <response code="401">El user id o el id task no se encontro.</response>
        /// <response code="401">No autorizado. Se requiere un token válido.</response>
        [Authorize(Policy = "UpdateOrUpdateAllPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> updateTask(
            [FromBody]UpdateTaskDto updateTaskDto,
            [FromRoute]int id
        ){
            try{
                var updateTask = await taskService.update(id,updateTaskDto);

                return StatusCode(201, new {
                    status = 201,
                    message = "Entity was update Successfully.",
                    data = updateTask
                });
            }catch(ArgumentException){
                throw new UnexpectedErrorException("Unexpected Error");
            }

        }

        /// <summary>
        /// Obtiene todas las tareas básicas sin filtros o paginación.
        /// </summary>
        /// <returns>Una lista de tareas básicas.</returns>
        /// <response code="200">Lista de tareas básicas.</response>
        /// <response code="401">No autorizado. Se requiere un token válido.</response>
        [HttpGet("findTask")]
        public async Task<IActionResult> findAllBase(){
            try{
                var foundTask = await taskService.findAllBase();
                return Ok(foundTask);   
            }
            catch (ArgumentException){
                throw new UnexpectedErrorException("Unexpected Error");
            }
        }

        /// <summary>
        /// Obtiene una tarea por su ID, junto con los detalles del usuario asignado.
        /// </summary>
        /// <param name="id">ID de la tarea que se desea obtener.</param>
        /// <returns>La tarea encontrada, junto con la información del usuario asignado.</returns>
        /// <response code="200">Tarea encontrada junto con el usuario asignado.</response>
        /// <response code="404">No se encontró una tarea con el ID proporcionado.</response>
        [HttpGet("findTaskById/{id}")]
        public async Task<IActionResult> findByIdOrFailWithUser(int id){
            try{
                var foundTask = await taskService.findByIdOrFailWithUser(id);
                return Ok(foundTask);   
            }
            catch (ArgumentException){
                throw new UnexpectedErrorException("Unexpected Error");
            }
        }
    }
}