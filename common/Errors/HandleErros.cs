using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace TaskManagement.Middleware
{
    public class ExceptionHandle
    {
        private readonly RequestDelegate _next;

        public ExceptionHandle(RequestDelegate next){
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try{
                await _next(context);
            }
            catch (DbUpdateException ex){
                await HandleAlreadyExistException(context,ex);
            }
            
            catch (Exception ex){
                await HandleException(context, ex);
            }
        }

        //NOTE: Unexpected error
        private Task HandleException(HttpContext context, Exception ex){
    
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // Code 500

            var errorResponse = new{
                status = context.Response.StatusCode,
                message = "An unexpected error occurred. Please try again later."
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        //NOTE: AlreadyExist Data
        private Task HandleAlreadyExistException(HttpContext context, DbUpdateException ex){
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // Code 400

            var innerMessage = ex.InnerException?.Message;
            var errorResponse = new{
                status = context.Response.StatusCode,
                message = "A database error occurred. Please check the provided data." //Default message
            };

            if (!string.IsNullOrEmpty(innerMessage) && innerMessage.Contains("duplicate key value violates unique constraint")){
                var fieldName = GetFieldNameByConstraint(innerMessage);
                errorResponse = new{
                    status = context.Response.StatusCode,
                    message = fieldName != null ? $"Value for '{fieldName}' already exists." : "A unique constraint violation occurred."
                };
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        //NOTE: method for extract constraint from db
        private string? GetFieldNameByConstraint(string errorMessage){
            if (errorMessage.Contains("IX_Permission_Name")){
                return "Name"; 
            }

            return null; 
        }

    }
}
