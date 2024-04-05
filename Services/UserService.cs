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
            //check if it exists
            if (DoesUserExist(User.Username))
            {
                //if true, continue with authentication, store our user object
                UserModel foundUser = GetUserByUsername(User.Username);
                //check if password is correct
                if (VerifyUsersPassword(User.Password, foundUser.Hash, foundUser.Salt))
                {
                    //Ctrl . to include using statements- for all pasted code
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    //generates a new token and log user out after 30 minutes
                    var tokeOptions = new JwtSecurityToken(
                        issuer: "http://localhost:5000",
                        audience: "http://localhost:5000",
                        claims: new List<Claim>(), // Claims can be added here if needed
                        expires: DateTime.Now.AddMinutes(30), // Set token expiration time (e.g., 30 minutes), Logs you off automatically
                        signingCredentials: signinCredentials // Set signing credentials
                    );

                    // Generate JWT token as a string
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                    //returns toekn through http response with status of 200
                    Result = Ok(new { Token = tokenString });

                    //Token:
                    //asdfhjklags. = header
                    //kasfdsflafslkd. Payload: contains claims such as expiration time
                    //:sdflkjhgsl:. = signiture encrypts and comineds header and payload using secret key
                }
            }
            return Result;
        }
        public UserModel GetUserByUsername(string username)
        {
            return _context.UserInfo.SingleOrDefault(user => user.UserName == username);
        }

        public bool UpdateUser(UserModel UsertoUpdate)
        {
            _context.Update<UserModel>(UsertoUpdate);
            return _context.SaveChanges() != 0;
        }
    }
}
