
namespace TaskManagement.DTOs{

    public class PaginationTaskDto{
            public int Limit { get; set; } = 10; //Default 10
    
            public int Offset { get; set; } = 0;  //Start from 0
    
            public string Tittle { get; set; } = ""; //Filter Tittle
            public string OrderBy { get; set; } = "asc";  //Default asc

            public string status {get;set;} = ""; //Filter status

            public string user {get;set;} = ""; //Filter user
    }


}