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

        //NOTE: Find all tasks
        public async Task<List<object>> findAll(
            PaginationTaskDto paginationTaskDto
        ){

            //Find all tasks
            var foundTasks = await taskRepository.findAll();

            //If search tittle is not null, filter the tasks
            var filetTitle = foundTasks.Where(
                task => task.tittle.Contains(paginationTaskDto.Tittle)
            ).ToList();

            //If search status is not null, filter the tasks
            var filterStatus = filetTitle.Where(
                task => task.status.Contains(paginationTaskDto.status)
            ).ToList();

            //If search user is not null, filter the tasks
            var filterUser = filterStatus.Where(
                task => task.user.Name.Contains(paginationTaskDto.user)
            ).ToList();

            //Sorting the tasks
            var sortedTasks = paginationTaskDto.OrderBy.ToLower() == "asc"
                ? filterUser.OrderBy(task => task.tittle)
                : filterUser.OrderByDescending(task => task.tittle);


            //Pagination: Skip the offset and take the limit and mapping data
            var formatData = sortedTasks
                .Skip(paginationTaskDto.Offset)
                .Take(paginationTaskDto.Limit)
                .Select(task => new{
                    id = task.id,
                    tittle = task.tittle,
                    description = task.description,
                    status = task.status,
                    user= new {
                        userId = task.user.Id,
                        userName = task.user.Name
                    },
                    created_at = task.created_at,
                    updated_at = task.updated_at
                }).ToList();

            return formatData.Cast<object>().ToList();
        }
    }
}