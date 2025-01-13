using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TaskManagement.DTOs;
using TaskManagement.Interfaces;
using TaskManagement.Entity;

namespace TaskManagement.Pages{
    public class CreateTaskModel : PageModel{
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserService _userService;

        public CreateTaskModel(
            IHttpClientFactory httpClientFactory,
            IUserService userService
        ){
            _httpClientFactory = httpClientFactory;
            _userService = userService;
        }

        [BindProperty]
        public string Tittle { get; set; }
        [BindProperty]
        public string? Description { get; set; }
        [BindProperty]
        public string Status { get; set; }
        [BindProperty]
        public int User { get; set; }

        public string? Message { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public async Task OnGetAsync(){
            try{
                Users = await _userService.GetAllUsersAsync();
            }
            catch (Exception ex)
            {
                Message = $"An error occurred while fetching users: {ex.Message}";
            }
        }

        public async Task<IActionResult> OnPostAsync(){
            var token = Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(token)){
                Message = "No valid token found.";
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var url = "http://localhost:5025/api/tasks";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var taskData = new CreateTaskDto{
                tittle = Tittle,
                description = Description,
                status = Status,
                user = User
            };

            var content = new StringContent(JsonSerializer.Serialize(taskData), Encoding.UTF8, "application/json");
            request.Content = content;

            try{
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Message = "Task created successfully!";
                    return RedirectToPage("/task/list/list");
                }
                else
                {
                    Message = $"Failed to create task. Status code: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                Message = $"An error occurred while creating the task: {ex.Message}";
            }

            return Page();
        }
    }
}
