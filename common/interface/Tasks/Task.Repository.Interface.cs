
using TaskManagement.Entity;

namespace TaskManagement.Interfaces{

    public interface ITaskRepository{
     Task<TaskEntity> add(TaskEntity taskEntity);   
    }
}