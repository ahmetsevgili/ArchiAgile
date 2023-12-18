using ArchiAgile.Server.Data;
using ArchiAgile.Server.Data.Entity;
using ArchiAgile.Server.Interfaces;
using ArchiAgile.Shared.User;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArchiAgile.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class SrvUserController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IUserService _userService;
        private int _userId => !string.IsNullOrWhiteSpace(User.Claims.FirstOrDefault(x => x.Type == "RecordID")?.Value) ?
    int.Parse(User.Claims.FirstOrDefault(x => x.Type == "RecordID")?.Value) : 0;
        public SrvUserController( ApplicationDBContext context,
                                  IMapper mapper,
                                  IWebHostEnvironment env,
                                  IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
            _userService = userService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public GetUserOnInitializedResponse GetUserOnInitialized()
        {
            return _userService.GetUserOnInitialized(_userId);
        }
        [HttpPost]
        public SaveUserResponse SaveUser(SaveUserRequest request)
        {
            _context.Database.BeginTransaction();
            
            var response = _userService.SaveUser(request);

            _context.Database.CommitTransaction();
            return response;
        }
    }
}
