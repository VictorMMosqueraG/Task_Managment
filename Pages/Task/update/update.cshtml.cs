using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TaskManagement.DTOs;
using TaskManagement.Interfaces;
using TaskManagement.Entity;

namespace TaskManagement.Pages{
    public class UpdateTaskModel : PageModel{
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserService _userService;
        private readonly ITaskService _taskService;

        public UpdateTaskModel(
            IHttpClientFactory httpClientFactory,
            IUserService userService,
            ITaskService taskService    
        ){
            _httpClientFactory = httpClientFactory;
            _userService = userService;
            _taskService = taskService;
        }

        [BindProperty]
        public int TaskId { get; set; }

        [BindProperty]
        public string? Tittle { get; set; }

        [BindProperty]
        public string? Description { get; set; }

        [BindProperty]
        public string? Status { get; set; }

        [BindProperty]
        public int User { get; set; }

        public string? Message { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public List<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();

        //NOTE: Get all Task and Users
        public async Task OnGetAsync(int id){
            TaskId = id;

            try{
                //Get all users
                Users = await _userService.GetAllUsersAsync();
                
                //GEt all task
                Tasks = await _taskService.findAllBase();

                
                var task = await _taskService.findByIdOrFail(id);

                if (task != null){
                    Tittle = task.tittle;
                    Description = task.description;
                    Status = task.status;
                    User = task.user.Id; 
                }
            }
            catch (Exception ex)
            {
                Message = $"An error occurred while fetching the task details: {ex.Message}";
            }
        }

        //NOTE: Method Update
        public async Task<IActionResult> OnPostAsync(){
            //Get token
            var token = Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(token)){
                Message = "No valid token found.";
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var url = $"http://localhost:5025/api/tasks/{TaskId}";  
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var taskData = new UpdateTaskDto{
                tittle = Tittle,
                description = Description,
                status = Status,
                user = User
            };

            var content = new StringContent(JsonSerializer.Serialize(taskData), Encoding.UTF8, "application/json");
            request.Content = content;

            try{
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode){
                    Message = "Task updated successfully!";
                    return RedirectToPage("/task/list/list");
                }else{
                    Message = $"Failed to update task. Status code: {response.StatusCode}";
                }
            }catch (Exception ex){
                Message = $"An error occurred while updating the task: {ex.Message}";
            }

            return Page();
        }



    }
}
