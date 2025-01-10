using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

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
            catch (AlreadyExistException ex){
                await HandleAlreadyExistException(context,ex);
            }
            catch (NotFoundException ex){
                await HandleNotFoundException(context, ex);
            }
            catch (UnexpectedErrorException ex){
                await UnexpectedErrorException(context, ex);
            }
        }

        //NOTE: Unexpected error
        private Task UnexpectedErrorException(HttpContext context, UnexpectedErrorException ex){
    
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // Code 500

            var errorResponse = new{
                status = context.Response.StatusCode,
                message = ex.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    
        //NOTE: AlreadyExist Data
        private Task HandleAlreadyExistException(HttpContext context, AlreadyExistException ex){
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // Code 400

            var errorResponse = new{
                status = context.Response.StatusCode,
                message = ex.Message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        //NOTE: NOT FOUND EXCEPTION
        private Task HandleNotFoundException(HttpContext context, NotFoundException ex){
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 404; 

            var errorResponse = new{
                status = context.Response.StatusCode,
                message = ex.Message 
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

       

    }
}
