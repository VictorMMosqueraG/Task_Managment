

using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Pages{

    public class DeleteTaskModel : PageModel{

        private readonly IHttpClientFactory httpClientFactory;

        private readonly ITaskService taskService;

        public DeleteTaskModel(
            IHttpClientFactory httpClientFactory,
            ITaskService taskService
        ){
            this.httpClientFactory = httpClientFactory;
            this.taskService = taskService;
        }

        [BindProperty]
        public int taskId { get; set; }

        public List<TaskEntity> tasks {get;set;} = new List<TaskEntity>();

        [BindProperty]
        public string? Message { get; set; }


        //NOTE: get all task
        public async Task OnGetAsync(int id){
            taskId = id;

            try{
                
                //Get all task
                tasks = await taskService.findAllBase();

                var onlyTask = taskService.findByIdOrFail(id);
            }catch (System.Exception){
                throw;
            }
        }


        public async Task<IActionResult> OnPostAsync(){
            //Get token
            var token = Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(token)){
                Message = "No valid token found.";
                return Page();
            }

            var client = httpClientFactory.CreateClient();
            var url = $"http://localhost:5025/api/tasks/{taskId}";  
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

           try{
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode){
                    Message = "Task delete successfully!";
                    return RedirectToPage("/task/list/list");
                }else{
                    Message = $"Failed to delete task. Status code: {response.StatusCode}";
                }
           }catch (Exception ex){
             Message = $"An error occurred while updating the task: {ex.Message}";
           }
           
           return Page();
        }


    }
}