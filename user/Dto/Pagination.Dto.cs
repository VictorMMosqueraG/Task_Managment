
namespace TaskManagement.DTOs{

    public class PaginationUserDto{
         public int Limit { get; set; } = 10; //Default 10

        public int Offset { get; set; } = 0;  //Start from 0

          public string Email { get; set; } = ""; //Filter Email
        public string OrderBy { get; set; } = "asc";  //Default asc

    }
}