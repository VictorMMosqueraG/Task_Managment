
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Repositories{

    public class TaskRepository:ITaskRepository{

        private readonly ApplicationDbContext context;

        public TaskRepository(ApplicationDbContext applicationDbContext){
            context = applicationDbContext;
        }

        public async Task<TaskEntity> add(TaskEntity taskEntity){
            context.Tasks.Add(taskEntity);
            await context.SaveChangesAsync();
            return taskEntity;
        }

        // Find task all
        public async Task<List<TaskEntity>> findAll(){
            var tasks = await context.Tasks
                .Include(x => x.user)
                .ToListAsync();

            return tasks;
        }

        public async Task<TaskEntity> delete(int id){
            //find by id, if not found throw exception
            var task = await findByIdOrFail(id);

            context.Tasks.Remove(task);
            await context.SaveChangesAsync();

            return task;
        }

        public async Task<TaskEntity> update(TaskEntity taskEntity){
            // Mark the entity as modified
            context.Entry(taskEntity).State = EntityState.Modified;

            // Save changes to the database
            await context.SaveChangesAsync();
            return taskEntity;
        }



        //NOTE: Base MEthods
        public async Task<TaskEntity> findByIdOrFail(int id){
            
            var task = await context.Tasks.FindAsync(id);

            if(task == null){
                throw new NotFoundException(id,"TASK");
            }
            return task;
        }

      
        public async Task<TaskEntity> findByIdOrFaiWithUser(int id){
            var task = await context.Tasks
                            .Include(t => t.user) 
                            .FirstOrDefaultAsync(t => t.id == id);

            if (task == null){
                throw new NotFoundException(id, "TASK");
            }
            return task;
        }

        
    }
}