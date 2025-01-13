using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskManagement.DTOs;
using TaskManagement.Interfaces;

namespace TaskManagement.Pages{
    public class RegisterModel : PageModel{
        private readonly IAuthService _authService;

        public RegisterModel(IAuthService authService){
            _authService = authService;
        }

        // Bind the property to the form input
        [BindProperty]
        public new CreateUserDto? User { get; set; }
        public string? Message { get; set; } 

        public async Task<IActionResult> OnPostAsync(){
            // If the form model state is invalid, return the page with an error message
            if (!ModelState.IsValid){
                Message = "Please complete all fields correctly.";
                return Page(); // Reload the page with the error message
            }

            try{
                // Call the service to create the user
                var result = await _authService.createUser(User!);

                // If the user is created successfully, set a success message and redirect to Home
                TempData["SuccessMessage"] = "User registered successfully.";
                TempData.Keep("SuccessMessage");

                return RedirectToPage("/Home/Home");
            }
            catch(DbUpdateException){
                Message = "The email address is already in use. Please choose a different one.";
                return Page();
            }
            catch (ArgumentException){
                Message = "An unexpected error occurred. Please try again.";
                return Page();
            }
        }
    }
}
