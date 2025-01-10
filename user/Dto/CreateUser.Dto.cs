namespace TaskManagement.DTOs{

    public class CreateUserDto{

        public required string name {get;set;}
        public required string email {get;set;}
        public required string password {get;set;}
        public required int role {get;set;}
    }
}