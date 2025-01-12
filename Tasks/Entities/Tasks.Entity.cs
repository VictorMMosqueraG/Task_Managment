
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Entity{

    public class TaskEntity{

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }

        [Required(ErrorMessage = "The tittle field is required.")]
        [StringLength(40)]
        public required string tittle { get; set; }

        [StringLength(200)]
        public string? description { get; set; }

        [Required(ErrorMessage = "The status field is required.")]
        public required string status { get; set; }

        public required User user { get; set; }

        public required DateTime created_at { get; set; }

        public DateTime? updated_at { get; set; }   

    }
}