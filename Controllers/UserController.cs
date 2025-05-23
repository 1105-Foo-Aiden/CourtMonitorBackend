using CourtMonitorBackend.Models;
using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourtMonitorBackend.Controllers{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase{
        private readonly UserService _data;
        public UserController(UserService data){
            _data = data;
        }

        [HttpGet]
        [Route("GetUserByUsername/{username}")]

        public UserDTO GetUserByUserName(string username){
            return _data.SearchUserByUserName(username);
        }

        [HttpGet]
        [Route("GetUserById/{id}")]
        public UserModel GetUserById(int id){
            return _data.GetUserById(id);
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IEnumerable<UserModel> GetAllUsers(){
            return _data.GetAllUsers();
        }
        
        [HttpGet]
        [Route("GetUserByEmail/{Email}")]
        public UserModel GetUserByEmail(string Email){
            return _data.GetUserByEmail(Email);
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
        // [Route("UpdateUserStatus/{username}/StatusToUpdate")]
        // public bool ChangeStatus(string username, string StatusToUpdate){
        //     return _data.ChangeStatus(username, StatusToUpdate);
        // }
        
        [HttpPost]
        [Route("ResetPassword")]
        public bool CreateNewPassword(ResetPasswordDTO NewPassword){
            return _data.ResetPassword(NewPassword);
        }

        [HttpPut]
        [Route("UpdateUser")]

        public bool UpdateUser([FromBody] UpdateUserDTO updateUser){
            return _data.UpdateUser(updateUser);
        }

        [HttpDelete]
        [Route("Deleteuser/{UserToDelete}")]
        public string Deleteuser(string UserToDelete){
            return _data.Deleteuser(UserToDelete);
        }
    }
}