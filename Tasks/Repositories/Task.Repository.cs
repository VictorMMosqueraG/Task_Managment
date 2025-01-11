
using TaskManagement.Data;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Repositories{

    public class TaskRepository:ITaskRepository{

      private readonly ApplicationDbContext context;

      public TaskRepository(ApplicationDbContext applicationDbContext){
          this.context = applicationDbContext;
      }

        public async Task<TaskEntity> add(TaskEntity taskEntity){
            context.Tasks.Add(taskEntity);
            await context.SaveChangesAsync();
            return taskEntity;
        }
    }
}