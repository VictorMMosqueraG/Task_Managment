using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskManagement.DTOs;
using TaskManagement.Interfaces;

namespace TaskManagement.Pages{
    public class LoginModel : PageModel{
        private readonly IAuthService _authService;

        public LoginModel(IAuthService authService){
            _authService = authService;
        }

        [BindProperty]
        public  LoginDto? LoginDto { get; set; }
        public string? Message { get; set; }

        public async Task<IActionResult> OnPostAsync(){
            if (!ModelState.IsValid){
                Message = "Invalid input";
                return Page();
            }

            try{
                
                var token = await _authService.loginUser(LoginDto!.Email, LoginDto.Password);

                Response.Cookies.Append("jwtToken", token, new CookieOptions{
                    HttpOnly = true, 
                    Secure = true,   
                    Expires = DateTime.UtcNow.AddDays(1)
                });

                return RedirectToPage("/Task/List/List");

            }
            catch (UnauthorizedAccessException){
                Message = "Invalid credentials.";
                return Page();
            }
            catch (Exception ){
                Message = $"Error Credentials";
                return Page();
            }
        }
    }
}
