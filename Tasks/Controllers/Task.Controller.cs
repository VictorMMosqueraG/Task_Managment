
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

        [Authorize(Policy = "WriteAllPolicy")]//Admin
        [Authorize(Policy = "WritePolicy")]//User
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

        [Authorize(Policy = "ReadAllPolicy")]//Admin
        [Authorize(Policy = "ReadPolicy")]//User
        [HttpGet]
        public async Task<IActionResult> findAllTask(
            [FromQuery] PaginationTaskDto paginationTaskDto
        ){
            var foundTasks = await taskService.findAll(paginationTaskDto);
            return Ok(foundTasks);
        }

        
        [Authorize(Policy = "DeleteAllPolicy")]//admin  
        [Authorize(Policy = "DeletePolicy")]//user
        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteTask(
            [FromRoute] int id
        ){
            var foundTask = await taskService.delete(id);
            return Ok(foundTask);
        }
    }
}