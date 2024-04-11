using CourtMonitorBackend.Models;
using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services.Context;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace CourtMonitorBackend.Services
{
    public class UserService : ControllerBase
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }
        public bool DoesUserExist(string Username)
        {
            return _context.UserInfo.SingleOrDefault(User => User.UserName == Username) != null;
        }

        public bool AddUser(CreateAccountDTO UserToAdd)
        {
            bool result = false;

            if (!DoesUserExist(UserToAdd.UserName))
            {
                UserModel newUser = new();
                var hashPassword = HashPassword(UserToAdd.Password);
                //setting up user
                newUser.ID = UserToAdd.ID;
                newUser.UserName = UserToAdd.UserName;
                newUser.Email = UserToAdd.Email;
                newUser.RealName = UserToAdd.FullName;
                newUser.Salt = hashPassword.Salt;
                newUser.Hash = hashPassword.Hash;
                _context.Add(newUser);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }

        public PassWordDTO HashPassword(string passowrd)
        {
            PassWordDTO newHashPassword = new();
            byte[] SaltByte = new byte[64];
            RNGCryptoServiceProvider provider = new();
            provider.GetNonZeroBytes(SaltByte);
            string salt = Convert.ToBase64String(SaltByte);
            Rfc2898DeriveBytes rfc2898DeriveBytes = new(passowrd, SaltByte, 10000);
            string hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            newHashPassword.Salt = salt;
            newHashPassword.Hash = hash;
            return newHashPassword;
        }

        public bool VerifyUsersPassword(string? passowrd, string? storedHash, string? storedSalt)
        {
            byte[] SaltBytes = Convert.FromBase64String(storedSalt);
            Rfc2898DeriveBytes rfc2898DeriveBytes = new(passowrd, SaltBytes, 10000);
            string newHash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            return newHash == storedHash;
        }

        public IActionResult Login(LoginDTO User)
        {
            IActionResult Result = Unauthorized();
            if (DoesUserExist(User.Username))
            {
                UserModel foundUser = GetUserByUsername(User.Username);
                if (VerifyUsersPassword(User.Password, foundUser.Hash, foundUser.Salt))
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var tokeOptions = new JwtSecurityToken(
                        issuer: "http://localhost:5000",
                        audience: "http://localhost:5000",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: signinCredentials
                    );
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    Result = Ok(new { Token = tokenString });
                }
            }
            return Result;
        }
        public UserModel GetUserByUsername(string username)
        {
            return _context.UserInfo.SingleOrDefault(user => user.UserName == username);
        }

        public bool UpdateUser(string UsertoUpdate, string updatebirthday, string updateimage, string updateprograms, string updatefunfact, string updateemail, string updateSports, string updateRealName)
        {
            UserModel foundUser = GetUserByUsername(UsertoUpdate);
            bool result = false;
            if (foundUser != null)
            {
                foundUser.Birthday = updatebirthday;
                foundUser.Image = updateimage;
                foundUser.Programs = updateprograms;
                foundUser.FunFact = updatefunfact;
                foundUser.Email = updateemail;
                foundUser.Sports = updateSports;
                foundUser.RealName = updateRealName;
                _context.Update<UserModel>(foundUser);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }

        public string Deleteuser(string userToDelete)
        {
            UserModel foundUser = GetUserByUsername(userToDelete);
            string result = "Not Found";
            if (foundUser != null)
            {
                _context.Remove<UserModel>(foundUser);
                _context.SaveChanges();
                result = "Found";
            }
            return result;
        }
        public UseridDTO GetUserIDByUserName(string username)
        {
            UseridDTO UserInfo = new();
            UserModel foundUser = _context.UserInfo.SingleOrDefault(user => user.UserName == username);
            UserInfo.Username = foundUser.UserName;
            UserInfo.Id = foundUser.ID;
            return UserInfo;
        }

        public UseridDTO GetUserById(int id)
        {
            UseridDTO UserInfo = new();
            UserModel foundUser = _context.UserInfo.SingleOrDefault(user => user.ID == id);
            UserInfo.Username = foundUser.UserName;
            UserInfo.Id = foundUser.ID;
            return UserInfo;
        }

        public bool ChangeStatus(string username, string StatusToUpdate)
        {
            UserModel foundUser = GetUserByUsername(username);

            bool result = false;
            if (foundUser != null)
            {
                switch (StatusToUpdate.ToLower())
                {
                    case "admin":
                        foundUser.IsAdmin = !foundUser.IsAdmin;
                        if(foundUser.IsAdmin){
                           AdminModel? admin = _context.AdminInfo.FirstOrDefault(admin => admin.UserID == foundUser.ID);
                           if(admin != null){
                            admin = new AdminModel {UserID = foundUser.ID};
                            _context.AdminInfo.Add(admin);
                           }
                        }
                        else{
                            AdminModel? admin = _context.AdminInfo.SingleOrDefault(admin => admin.UserID == foundUser.ID);
                            if (admin != null){
                                _context.AdminInfo.Remove(admin);
                            }
                        }
                    break;
                    case "coach":
                        foundUser.IsCoach = !foundUser.IsCoach;
                        if(foundUser.IsCoach){
                            CoachModel? coach = _context.CoachInfo.SingleOrDefault(coach => coach.UserID == foundUser.ID);
                            if(coach != null){
                                coach = new CoachModel {UserID = foundUser.ID};
                                _context.CoachInfo.Add(coach);
                            }
                            else{

                            }
                        } 
                    break;
                    case "genuser":
                        foundUser.IsUser = !foundUser.IsUser;
                    break;
                }
                _context.Update<UserModel>(foundUser);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }
    }
}
