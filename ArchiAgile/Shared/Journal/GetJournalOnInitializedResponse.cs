using ArchiAgile.Shared.Personnel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiAgile.Shared.Journal
{
    public class GetJournalOnInitializedResponse : BaseResponse
    {
        public List<PersonnelDTO> PersonnelList { get; set; }
        public List<PersonnelRoleDTO> PersonnelRoleList { get; set; }
    }
}
