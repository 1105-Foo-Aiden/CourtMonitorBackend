using CourtMonitorBackend.Models;
using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services.Context;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CourtMonitorBackend.Services
{
    public class UserService : ControllerBase
    {
        private readonly DataContext _context;

        public UserService(DataContext context){
            _context = context;
        }
        public bool DoesUserExist(string Username){
            return _context.UserInfo.SingleOrDefault(User => User.UserName == Username) != null;
        }

        public bool AddUser(CreateAccountDTO UserToAdd){
            bool result = false;

            if (!DoesUserExist(UserToAdd.UserName)){
                UserModel newUser = new()
                {
                    ID = UserToAdd.ID,
                    UserName = UserToAdd.UserName,
                    Email = UserToAdd.Email,
                    RealName = UserToAdd.FullName
                };
                //setting up user
                var hashPassword = HashPassword(UserToAdd.Password);

                newUser.Salt = hashPassword.Salt;
                newUser.Hash = hashPassword.Hash;
                _context.Add(newUser);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }

        public PassWordDTO HashPassword(string passowrd){
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

        public bool VerifyUsersPassword(string? passowrd, string? storedHash, string? storedSalt){
            byte[] SaltBytes = Convert.FromBase64String(storedSalt);
            Rfc2898DeriveBytes rfc2898DeriveBytes = new(passowrd, SaltBytes, 10000);
            string newHash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
            return newHash == storedHash;
        }

        public IActionResult Login(LoginDTO User){
            IActionResult Result = Unauthorized();
            if (DoesUserExist(User.Username)){
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
        public UserDTO SearchUserByUserName(string username){
            UserModel foundUser = _context.UserInfo.SingleOrDefault(x => x.UserName == username);
            UserDTO searchedUser = new(){
                Username = foundUser.UserName,
                RealName = foundUser.RealName,
                Programs = foundUser.Programs,
                FunFact = foundUser.FunFact,
                Birthday = foundUser.Birthday,
                Email = foundUser.Email,
                Sports = foundUser.Sports,
                Image = foundUser.Image,
                IsAdmin = foundUser.IsAdmin,
                IsUser = foundUser.IsUser,
                IsCoach = foundUser.IsCoach,
                UserID = foundUser.ID
            };
            return searchedUser;
        }
        public UserModel GetUserByUsername(string username){
            return _context.UserInfo.FirstOrDefault(x => x.UserName == username);
        }
        public UserModel GetUserByEmail(string Email){
            return _context.UserInfo.FirstOrDefault(x => x.Email == Email);
        }

        public bool UpdateUser(string UsertoUpdate, string updatebirthday, string updateimage, string updateprograms, string updatefunfact, string updateemail, string updateSports, string updateRealName){
            UserModel foundUser = GetUserByUsername(UsertoUpdate);
            bool result = false;
            if (foundUser != null){
                if (updatebirthday != null){
                    foundUser.Birthday = updatebirthday;
                }
                if (updateimage != null){
                    foundUser.Image = updateimage;
                }
                if (updateprograms != null){
                    foundUser.Programs = updateprograms;
                }
                if (updatefunfact != null){
                    foundUser.FunFact = updatefunfact;
                }
                if (updateemail != null){
                    foundUser.Email = updateemail;
                }
                if (updateSports != null){
                    foundUser.Sports = updateSports;
                }
                if (updateRealName != null){
                    foundUser.RealName = updateRealName;
                }
                _context.Update<UserModel>(foundUser);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }

        public string Deleteuser(string userToDelete){
            UserModel foundUser = GetUserByUsername(userToDelete);
            string result = "Not Found";
            if (foundUser != null){
                _context.Remove<UserModel>(foundUser);
                _context.SaveChanges();
                result = "Found";
            }
            return result;
        }

        public UseridDTO GetUserIDByUserName(string username){
            UseridDTO UserInfo = new();
            UserModel foundUser = _context.UserInfo.SingleOrDefault(user => user.UserName == username);
            UserInfo.Username = foundUser.UserName;
            UserInfo.Id = foundUser.ID;
            return UserInfo;
        }

        public UserModel GetUserById(int id){
            return _context.UserInfo.SingleOrDefault(user => user.ID == id);
        }

        // public bool ChangeStatus(string username, string StatusToUpdate){
        //     UserModel foundUser = GetUserByUsername(username);
        //     bool result = false;
        //     if (foundUser != null){
        //         switch (StatusToUpdate.ToLower()){
        //             case "admin":
        //                 foundUser.IsAdmin = !foundUser.IsAdmin;
        //                 if (foundUser.IsAdmin){
                            //When we create an admin, we want to create a new Row in the Admin table
                            //if the User's ID already exists in the table, then we want to just add it to the table, 
                            //instsead of creating a new row
                            //Add the User's ID in the Admin Table as the AdminID
                            //Leave the ProgramID blank so we can update and add to it later
        //                     AdminModel? admin = _context.AdminInfo.FirstOrDefault(admin => admin.UserID == foundUser.ID);
        //                     if (admin != null){
        //                         admin = new AdminModel { UserID = foundUser.ID };
        //                         _context.AdminInfo.Add(admin);
        //                     }
        //                 }
        //                 else{
                            //find the admin ID
                            //find the specific Program they're being removed from (if they have multiple)
                            //Remove number from table and if that was the last number(probably a .filter),
                            //Remove the entire row instead
                            //change IsAdmin to False ONLY IF THEY HAVE NO MORE ADMINS if(ID's == null or .length)?
        //                     AdminModel? admin = _context.AdminInfo.SingleOrDefault(admin => admin.UserID == foundUser.ID);
        //                     if (admin != null){
        //                         _context.AdminInfo.Remove(admin);
        //                     }
        //                 }
        //                 break;
        //             case "coach":
        //                 foundUser.IsCoach = !foundUser.IsCoach;
        //                 if (foundUser.IsCoach){
        //                     CoachModel? coach = _context.CoachInfo.SingleOrDefault(coach => coach.UserID == foundUser.ID);
        //                     if (coach != null){
        //                         coach = new CoachModel { UserID = foundUser.ID };
        //                         _context.CoachInfo.Add(coach);
        //                     }
        //                     else{
        //                     }
        //                 }
        //                 break;
        //             case "genuser":
        //                 foundUser.IsUser = !foundUser.IsUser;
        //                 break;
        //         }
        //         _context.Update<UserModel>(foundUser);
        //         result = _context.SaveChanges() != 0;
        //     }
        //     return result;
        // }
        public IEnumerable<UserModel> GetAllUsers(){
            return _context.UserInfo;
        }

        public bool ResetPassword(ResetPasswordDTO NewPassword){
            bool result = false;
            UserModel foundUser = GetUserByEmail(NewPassword.Email);
            if (foundUser != null){
                var newPass = HashPassword(NewPassword.NewPassword);
                if(newPass.Hash == foundUser.Hash || newPass.Salt == foundUser.Salt){
                    result = false;
                }
                else{
                    foundUser.Hash = newPass.Hash;
                    foundUser.Salt = newPass.Salt;
                    _context.Update<UserModel>(foundUser);
                    result = _context.SaveChanges() != 0;
                }
            }
            return result;
        }

        public bool CreateAdminByID(int id){
            bool result = false;
            AdminModel? newAdmin = _context.AdminInfo.FirstOrDefault(x => x.UserID == id);
            if (newAdmin != null){
                _context.AdminInfo.Add(newAdmin);
                result = _context.SaveChanges() != 0;
            }

            return result;
        }

        // public bool CreateAdmin(string username){
        //     bool result = false;
        //     UserModel foundUser = GetUserByUsername(username);
        //     if (foundUser != null && foundUser.IsAdmin){
        //         AdminModel NewAdmin = new()
        //         {
        //             UserID = foundUser.ID,
        //         };
        //         _context.Add(NewAdmin);
        //         result = _context.SaveChanges() != 0;
        //     };
        //     return result;
        // }

        // public bool RemoveAdmin(string username){
        //     bool result= false;

        //         UserModel foundUser = GetUserByUsername(username);
        //         AdminModel foundAdmin = _context.AdminInfo.SingleOrDefault(x => x.UserID == foundUser.ID);
        //         if (foundAdmin != null){
        //             _context.Remove(foundAdmin);
        //             result = _context.SaveChanges() !=0;
        //         }
        //     return result;
        // }
    }
}
