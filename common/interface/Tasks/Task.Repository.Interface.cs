
using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public interface ITaskRepository{
        Task<TaskEntity> add(TaskEntity taskEntity);

        Task<List<TaskEntity>> findAll();

        Task<TaskEntity> delete(int id);

        Task<TaskEntity> update(TaskEntity taskEntity);

        Task<TaskEntity> findByIdOrFail(int id);
        Task<TaskEntity> findByIdOrFaiWithUser(int id);
    }
}