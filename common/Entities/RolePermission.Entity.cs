using System.Text.Json.Serialization;

namespace TaskManagement.Entity{

    public class RolePermission{

        [JsonIgnore]
        public int RoleId { get; set; }

        [JsonIgnore]
        public  Role? Role { get; set; }

        [JsonIgnore]
        public int PermissionId { get; set; }
        public  Permission? Permission { get; set; }
    }
}