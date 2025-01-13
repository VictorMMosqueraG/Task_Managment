using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using TaskManagement.DTOs;

namespace TaskManagement.Pages{
    public class TaskListModel : PageModel{
        private readonly IHttpClientFactory _httpClientFactory;

        public TaskListModel(IHttpClientFactory httpClientFactory){
            _httpClientFactory = httpClientFactory;
        }

        public List<Dictionary<string, object>>? Tasks { get; set; }
        public string? Message { get; set; }

        [BindProperty(SupportsGet = true)]
        public PaginationTaskDto Pagination { get; set; } = new PaginationTaskDto();

        //NOTE: Get Task
        public async Task OnGetAsync(){
            var token = Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(token)){
                Message = "No valid token found.";
                return;
            }

            var client = _httpClientFactory.CreateClient();

            var url = "http://localhost:5025/api/tasks";

            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(Pagination.Tittle)){
                queryParams.Add($"tittle={Pagination.Tittle}");
            }

            if (!string.IsNullOrEmpty(Pagination.status) && Pagination.status != "All"){
                queryParams.Add($"status={Pagination.status}");
            }

            if (!string.IsNullOrEmpty(Pagination.user)){
                queryParams.Add($"user={Pagination.user}");
            }

            if (!string.IsNullOrEmpty(Pagination.OrderBy)){
                queryParams.Add($"orderBy={Pagination.OrderBy}");
            }

            queryParams.Add($"limit={Pagination.Limit}");
            queryParams.Add($"offset={Pagination.Offset}");

            if (queryParams.Any()){
                url += "?" + string.Join("&", queryParams);
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try{
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode){
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Tasks = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(responseBody);
                }else{
                    Message = $"Failed to load tasks. Status code: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                Message = $"An error occurred while fetching tasks: {ex.Message}";
            }
        }
    }
}
