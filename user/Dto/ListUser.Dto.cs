
using TaskManagement.Entity;

namespace TaskManagement.DTOs{

    public class ListUserDto{
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}