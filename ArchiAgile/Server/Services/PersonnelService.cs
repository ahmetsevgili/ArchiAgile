using ArchiAgile.Server.Data;
using ArchiAgile.Server.Data.Entity;
using ArchiAgile.Server.Interfaces;
using ArchiAgile.Server.Utils;
using ArchiAgile.Shared.Personnel;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ArchiAgile.Server.Services
{
    public class PersonnelService : IPersonnelService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        public PersonnelService(ApplicationDBContext dbContext,
                                IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public SavePersonnelResponse SavePersonnel(SavePersonnelRequest request)
        {
            var response = new SavePersonnelResponse();
            if (request.Personnel.RecordID > 0)
            {
                var personnel = _dbContext.Personnel.FirstOrDefault(q => q.RecordID == request.Personnel.RecordID);
                if (personnel == null)
                {
                    response.ResponseCode = "ASU1";
                    response.ResponseMessage = "Personnel is not found!";
                    return response;
                }
                personnel.Name = request.Personnel.Name;
                personnel.Surname = request.Personnel.Surname;
                personnel.IsActive = request.Personnel.IsActive;

                _dbContext.Attach(personnel).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbContext.SaveChanges();

            }
            else
            {
                var personnel = new Personnel
                {
                    IsActive = request.Personnel.IsActive,
                    Name = request.Personnel.Name,
                    Surname = request.Personnel.Surname,
                    PersonnelRoleID = request.Personnel.PersonnelRoleID,
                };
                _dbContext.Personnel.Add(personnel);
                _dbContext.SaveChanges();
            }

            return response;
        }
        public GetPersonnelOnInitializedResponse GetPersonnelOnInitialized()
        {
            var response = new GetPersonnelOnInitializedResponse();
            var personnelList = _dbContext.Personnel.ToList();
            response.PersonnelRoleList = _mapper.Map<List<PersonnelRoleDTO>>(_dbContext.PersonnelRole);
            response.PersonnelList = _mapper.Map<List<PersonnelDTO>>(personnelList);

            return response;
        }
    }
}
