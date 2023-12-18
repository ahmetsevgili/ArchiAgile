using ArchiAgile.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using ArchiAgile.Server.Data;
using ArchiAgile.Shared.User;

namespace ArchiAgile.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SrvAccountController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IWebHostEnvironment _env;
        public SrvAccountController(ApplicationDBContext dbContext,
                            IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }
        [HttpPost]
        public CurrentUserDTO CurrentUserInfo()
        {
            var currentUser = new CurrentUserDTO
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                Name = User.Claims.FirstOrDefault(x => x.Type == "Name")?.Value,
                Surname = User.Claims.FirstOrDefault(x => x.Type == "Surname")?.Value,
                Claims = User.Claims.ToDictionary(c => c.Type, c => c.Value),
                UserID = GetUserID(),
                Image = GetUserImage()
            };
            return currentUser;
        }
        private int GetUserID()
        {

            if (string.IsNullOrWhiteSpace(User.Claims.FirstOrDefault(x => x.Type == "RecordID")?.Value))
            {
                return 0;
            }
            else
            {
                return int.Parse(User.Claims.FirstOrDefault(x => x.Type == "RecordID")?.Value);
            }
        }

        private string GetUserImage()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrWhiteSpace(User.Claims.FirstOrDefault(x => x.Type == "ImagePath")?.Value))
                {
                    var userImagePath = _dbContext.Parameter.FirstOrDefault(q => q.Name == "USER_IMAGE_PATH");
                    if (userImagePath == null)
                    {
                        return "";
                    }
                    var imagePath = userImagePath.Value;
                    var path = Path.Combine(_env.WebRootPath, imagePath);
                    var file = Path.Combine(path, GetUserID().ToString());
                    if (System.IO.File.Exists(file))
                    {
                        return "data:image/jpeg;base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes(file));
                    }
                }
            }

            return "";
        }
        [HttpPost]
        public async Task<SignInResponse> SignIn(SignInRequest signInRequest)
        {
            var response = new SignInResponse();

            var user = _dbContext.User.FirstOrDefault(q => q.Username == signInRequest.Username && q.Password == ComputeSha256Hash(signInRequest.Password));

            if (user == null)
            {
                response.ResponseMessage = "Username or Password is wrong!";
                response.ResponseCode = "ASI1";
                return response;
            }

            if (!user.IsActive)
            {
                response.ResponseMessage = "User is not active!";
                response.ResponseCode = "ASI2";
                return response;
            }

            var userRole = _dbContext.UserRole.FirstOrDefault(q => q.RecordID == user.UserRoleID);

            if (userRole == null)
            {
                response.ResponseMessage = "User role is empty!";
                response.ResponseCode = "ASI3";
                return response;
            }

            var claims = new List<Claim>
                {
                    new Claim("RecordID", user.RecordID.ToString()),
                    new Claim("Name", user.Name),
                    new Claim("Surname", user.Surname),
                    new Claim(ClaimTypes.Name , user.Username),
                    new Claim(ClaimTypes.Role, userRole.Name),
                    new Claim("ImagePath", user.ImagePath??""),
                };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = signInRequest.Rememberme,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

            return response;
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<bool> SignOut()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}
