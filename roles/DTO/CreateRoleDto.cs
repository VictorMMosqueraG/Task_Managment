using System.Collections.Generic;

namespace TaskManagement.DTOs{
    public class CreateRoleDTO{
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<int> Permissions { get; set; } = new List<int>();
    }
}
