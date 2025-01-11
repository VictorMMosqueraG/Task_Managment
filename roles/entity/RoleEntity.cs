using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Entity{

    public class Role{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(40)]
        public required string Name { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }
        
        //Relationship many to many with permission
        public required ICollection<RolePermission> Permissions { get; set; } = new List<RolePermission>();
    }
}