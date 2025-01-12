
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

        [Authorize(Policy = "WriteAllPolicy")]
        [Authorize(Policy = "WritePolicy")]
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

        [HttpGet]
        public async Task<IActionResult> findAll(
            [FromQuery] PaginationTaskDto paginationTaskDto
        ){
            var foundTasks = await taskService.findAll(paginationTaskDto);
            return Ok(foundTasks);
        }


    }
}