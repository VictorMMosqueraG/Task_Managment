
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.DTOs;
using TaskManagement.Interfaces;

namespace TaskManagement.Controllers{

    [Route("api/user")]
    [ApiController]
    public class UserController:ControllerBase{

        private readonly IUserService service;

        public UserController(IUserService _service){
            service = _service;
        }
    }
}