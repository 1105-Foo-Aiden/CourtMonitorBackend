using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourtMonitorBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _data;
        public UserController(UserService data)
        {
            _data = data;
        }
        
        [HttpPost]
        [Route("AddUser")]
        public bool AddUser(CreateAccountDTO userToAdd){
            return _data.AddUser(userToAdd);
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginDTO User){
            return _data.Login(User);
        }

        // [HttpPost]
        // [Route("User/Login")]
        // public bool Login(LoginDTO userToAdd){
          
        // }
    }
}