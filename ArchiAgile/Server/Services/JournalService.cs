using ArchiAgile.Server.Data;
using ArchiAgile.Server.Data.Entity;
using ArchiAgile.Server.Interfaces;
using ArchiAgile.Shared.Journal;
using ArchiAgile.Shared.Personnel;
using AutoMapper;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace ArchiAgile.Server.Services
{
    public class JournalService : IJournalService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        public JournalService(ApplicationDBContext dbContext,
                           IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public GetJournalOnInitializedResponse GetJournalOnInitialized()
        {
            var response = new GetJournalOnInitializedResponse();
            var personnelList = _dbContext.Personnel.ToList();
            response.PersonnelRoleList = _mapper.Map<List<PersonnelRoleDTO>>(_dbContext.PersonnelRole);
            response.PersonnelList = _mapper.Map<List<PersonnelDTO>>(personnelList);

            return response;
        }
        public GetJournalPaginationResponse GetJournalPagination(GetJournalPaginationRequest request, int userID)
        {
            var response = new GetJournalPaginationResponse();
            IQueryable<Journal> query = _dbContext.Journal.Where(q => q.UserID == userID);

            if (!string.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(request.Filter);
            }

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                query = query.OrderBy(request.OrderBy);
            }

            response.Count = query.Count();
            var noteList = query.Skip((request.Skip ?? 0)).Take((request.Top ?? 100)).ToList();
            response.JournalList = _mapper.Map<List<JournalDTO>>(noteList);

            return response;
        }

        public SaveJournalResponse SaveJournal(SaveJournalRequest request, int userID)
        {
            var response = new SaveJournalResponse();
            if (request.Journal.RecordID > 0)
            {
                var note = _dbContext.Journal.FirstOrDefault(q => q.RecordID == request.Journal.RecordID);
                if (note == null)
                {
                    response.ResponseCode = "ASU1";
                    response.ResponseMessage = "Note is not found!";
                    return response;
                }
                note = _mapper.Map(request.Journal, note);

                _dbContext.Attach(note).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbContext.SaveChanges();

            }
            else
            {
                var note = _mapper.Map<Journal>(request.Journal);
                note.UserID = userID;
                _dbContext.Journal.Add(note);
                _dbContext.SaveChanges();
            }

            return response;
        }
    }
}
