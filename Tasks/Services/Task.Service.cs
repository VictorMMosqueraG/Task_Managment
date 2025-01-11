using TaskManagement.DTOs;
using TaskManagement.Entity;
using TaskManagement.Interfaces;
namespace TaskManagement.Services{

    public class TaskService : ITaskService{
        private readonly ITaskRepository taskRepository;
        private readonly IUserService userService;
        public TaskService(
            ITaskRepository _taskRepository,
            IUserService _userService)
        {
            taskRepository = _taskRepository;
            userService = _userService;
        }

        public async Task<TaskEntity> CreateTask(CreateTaskDto dto){

            //valid if user exist
            var foundUser = await userService.findByIdOrFail(dto.user);

            if(foundUser == null){
                throw new NotFoundException(dto.user,"USER");
            }

            if (!Constants.TaskStatus.AllStatuses.Contains(dto.status)){
                throw new EnumException("Invalid status value (Done, Pending, InProgress)");
            }

            //Mapping data
            var taskEntity = new TaskEntity{
                tittle = dto.tittle,
                description = dto.description,
                status = dto.status,
                user = foundUser,
                created_at = DateTime.UtcNow
            };
            
            return await taskRepository.add(taskEntity);
        }
    }
}