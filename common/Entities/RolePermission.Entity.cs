using System.Text.Json.Serialization;

namespace TaskManagement.Entity{

    public class RolePermission{

        public int RoleId { get; set; }

        public  Role? Role { get; set; }

        public int PermissionId { get; set; }
        public  Permission? Permission { get; set; }
    }
}