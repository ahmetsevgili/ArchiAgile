using ArchiAgile.Server.Data.Entity;
using ArchiAgile.Shared.Journal;
using ArchiAgile.Shared.Personnel;
using ArchiAgile.Shared.User;
using AutoMapper;
using Microsoft.VisualBasic;

namespace ArchiAgile.Server.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<UserRole, UserRoleDTO>();
            CreateMap<UserRoleDTO, UserRole>();

            CreateMap<Personnel, PersonnelDTO>();
            CreateMap<PersonnelDTO, Personnel>();

            CreateMap<PersonnelRole, PersonnelRoleDTO>();
            CreateMap<PersonnelRoleDTO, PersonnelRole>();

            CreateMap<Journal, JournalDTO>();
            CreateMap<JournalDTO, Journal>();
        }
    }
}
