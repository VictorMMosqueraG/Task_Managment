using Microsoft.AspNetCore.Mvc;
using TaskManagement.Entity;
using TaskManagement.Interfaces;

namespace TaskManagement.Controllers{

    [Route("api/permission")]
    [ApiController]
    public class PermissionController: ControllerBase{
        private readonly IPermissionService _service;

        public PermissionController(IPermissionService service){
            _service = service;
        }

        //NOTE: Save Permission
        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] Permission permission){
            try{
                var createdPermission = await _service.createPermission(permission);
                return CreatedAtAction(nameof(CreatePermission), new { id = createdPermission.Id }, createdPermission);
            }
            catch (ArgumentException ex){
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}