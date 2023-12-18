using ArchiAgile.Server.Data;
using ArchiAgile.Server.Data.Entity;
using ArchiAgile.Server.Interfaces;
using ArchiAgile.Server.Utils;
using ArchiAgile.Shared;
using ArchiAgile.Shared.User;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ArchiAgile.Server.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ICacheService _cacheService;
        public UserService(ApplicationDBContext dbContext, 
                           IMapper mapper,
                           IWebHostEnvironment env,
                           ICacheService cacheService)
        {
            _dbContext = dbContext;
            _env = env;
            _cacheService = cacheService;
            _mapper = mapper;
        }
        public SaveUserResponse SaveUser(SaveUserRequest request)
        {
            var response = new SaveUserResponse();
            if (request.User.RecordID > 0)
            {
                var user = _dbContext.User.FirstOrDefault(q => q.RecordID == request.User.RecordID);
                if (user == null)
                {
                    response.ResponseCode = "ASU1";
                    response.ResponseMessage = "User is not found!";
                    return response;
                }
                user.Name = request.User.Name;
                user.Surname = request.User.Surname;
                user.IsActive = request.User.IsActive;
                if (!string.IsNullOrWhiteSpace(request.User.Password))
                {
                    if (request.User.Password != request.User.Repassword)
                    {
                        response.ResponseCode = "ASU3";
                        response.ResponseMessage = "Password and Repassword missmatch!";
                        return response;
                    }
                    user.Password = Hash.ComputeSha256Hash(request.User.Password);
                }
                _dbContext.Attach(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbContext.SaveChanges();
                SaveUserImage(request, user);
            }
            else
            {
                var checkUser = _dbContext.User.FirstOrDefault(q => q.Username == request.User.Username);
                if (checkUser != null)
                {
                    response.ResponseCode = "ASU4";
                    response.ResponseMessage = "Username has been already used!";
                    return response;
                }
                var user = new User
                {
                    IsActive = request.User.IsActive,
                    Name = request.User.Name,
                    Password = Hash.ComputeSha256Hash(request.User.Password),
                    Surname = request.User.Surname,
                    Username = request.User.Username,
                    UserRoleID = request.User.UserRoleID,
                };
                _dbContext.User.Add(user);
                _dbContext.SaveChanges();
                SaveUserImage(request, user);

            }

            return response;
        }
        private BaseResponse SaveUserImage(SaveUserRequest request, User user)
        {
            var response = new BaseResponse();
            if (!string.IsNullOrWhiteSpace(request.User.Image))
            {
                var userImagePath = _cacheService.GetUserImagePath();
                if (string.IsNullOrWhiteSpace(userImagePath))
                {
                    response.ResponseCode = "ASU2";
                    response.ResponseMessage = "User image path is not found!";
                    return response;
                }

                var path = Path.Combine(_env.WebRootPath, userImagePath);

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                var file = Path.Combine(path, user.RecordID.ToString());
                System.IO.File.WriteAllBytes(file, Convert.FromBase64String(request.User.Image.Substring(request.User.Image.IndexOf(",") + 1)));
                user.ImagePath = Path.Combine(userImagePath, user.RecordID.ToString());
                _dbContext.SaveChanges();
            }

            return response;
        }
        public GetUserOnInitializedResponse GetUserOnInitialized(int userId)
        {
            var response = new GetUserOnInitializedResponse();
            var userList = _dbContext.User.Where(q => q.RecordID != userId).ToList();
            response.UserRoleList = _mapper.Map<List<UserRoleDTO>>(_dbContext.UserRole);
            response.UserList = new List<UserDTO>();

            foreach (var item in userList)
            {
                var userDTO = _mapper.Map<UserDTO>(item);
                userDTO.Password = "";
                userDTO.Repassword = "";

                response.UserList.Add(userDTO);

                if (!string.IsNullOrWhiteSpace(item.ImagePath))
                {
                    var userImagePath = _cacheService.GetUserImagePath();
                    if (string.IsNullOrWhiteSpace(userImagePath))
                    {
                        response.ResponseCode = "UGUOI1";
                        response.ResponseMessage = "User image path is not found!";
                        return response;
                    }
                    var path = Path.Combine(_env.WebRootPath, userImagePath);
                    var file = Path.Combine(path, item.RecordID.ToString());
                    if (System.IO.File.Exists(file))
                    {
                        userDTO.Image = "data:image/jpeg;base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes(file));
                    }
                }
            }

            return response;
        }
    }
}
