using ArchiAgile.Server.Data;
using ArchiAgile.Server.Interfaces;
using ArchiAgile.Shared.Personnel;
using ArchiAgile.Shared.User;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArchiAgile.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class SrvPersonnelController : Controller
    {
        private readonly IPersonnelService _personnelService;
        public SrvPersonnelController(IPersonnelService personnelService)
        {
            _personnelService = personnelService;
        }
        [HttpPost]
        public GetPersonnelOnInitializedResponse GetPersonnelOnInitialized()
        {
            return _personnelService.GetPersonnelOnInitialized();
        }

        [HttpPost]
        public SavePersonnelResponse SavePersonnel(SavePersonnelRequest request)
        {
            return _personnelService.SavePersonnel(request);
        }
    }
}
