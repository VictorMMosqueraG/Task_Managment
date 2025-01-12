
using TaskManagement.DTOs;
using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public interface ITaskService{
        Task<TaskEntity> CreateTask(CreateTaskDto createTaskDto);

        Task<List<object>> findAll(
            PaginationTaskDto paginationTaskDto
        );

        Task<TaskEntity> delete(int id);

        Task<TaskEntity> update(
            int id,
            UpdateTaskDto updateTaskDto
        );
    }
}