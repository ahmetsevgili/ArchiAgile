using ArchiAgile.Server.Interfaces;
using ArchiAgile.Server.Services;
using ArchiAgile.Shared.Journal;
using ArchiAgile.Shared.Personnel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArchiAgile.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class SrvJournalController : Controller
    {
        private readonly IJournalService _journalService;

        private int _userID => !string.IsNullOrWhiteSpace(User.Claims.FirstOrDefault(x => x.Type == "RecordID")?.Value) ?
                                int.Parse(User.Claims.FirstOrDefault(x => x.Type == "RecordID")?.Value) : 0;
        public SrvJournalController(IJournalService journalService)
        {
            _journalService = journalService;
        }

        [HttpPost]
        public GetJournalOnInitializedResponse GetJournalOnInitialized()
        {
            return _journalService.GetJournalOnInitialized();
        }

        [HttpPost]
        public GetJournalPaginationResponse GetJournalPagination(GetJournalPaginationRequest request)
        {
            return _journalService.GetJournalPagination(request, _userID);
        }

        [HttpPost]
        public SaveJournalResponse SaveJournal(SaveJournalRequest request)
        {
            return _journalService.SaveJournal(request, _userID);
        }
    }
}
