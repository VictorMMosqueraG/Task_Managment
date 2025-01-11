
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Entity{

    public class User{

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get;set;}

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(40)]
        public required string Name {get;set;}

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress]
        public required string Email {get;set;}

        [Required(ErrorMessage = "The password field is required.")]
        public required string Password {get;set;}

        //Relationship many to one -> role
        [Required(ErrorMessage = "The role field is required.")]
        public required Role role {get;set;}
    }
}