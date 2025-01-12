
using TaskManagement.Entity;

namespace TaskManagement.DTOs{

    public class CreateTaskDto{
        public required string tittle { get; set; }
        public string? description { get; set; }    

        public required string status { get; set; }

        public required int user { get; set; }
    }
}