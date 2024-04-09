using CourtMonitorBackend.Models;
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
        public bool AddUser(CreateAccountDTO userToAdd)
        {
            return _data.AddUser(userToAdd);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginDTO User)
        {
            return _data.Login(User);
        }
        [HttpGet]
        [Route("GetUserByUsername/{username}")]

        public UseridDTO GetUserByUserName(string username)
        {
            return _data.GetUserIDByUserName(username);
        }
        [HttpGet]
        [Route("GetUserById/{id}")]
        public UseridDTO GetUserById(int id)
        {
            return _data.GetUserById(id);
        }
        [HttpPut]
        [Route("UpdateUser/{UsertoUpdate}/birthday/image/programs/funfact/email/sports/realname")]

        public bool UpdateUser(string? UsertoUpdate, string? birthday, string? image, string? programs, string? funfact, string? email, string? sports, string realName)
        {
            return _data.UpdateUser(UsertoUpdate, birthday, image, programs, funfact, email, sports, realName);
        }

        [HttpDelete]
        [Route("Deleteuser/{UserToDelete}")]
        public string Deleteuser(string UserToDelete)
        {
            return _data.Deleteuser(UserToDelete);
        }





    }
}