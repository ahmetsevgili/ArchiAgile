using ArchiAgile.Shared.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiAgile.Shared.Personnel
{
    public class GetPersonnelOnInitializedResponse : BaseResponse
    {
        public List<PersonnelDTO> PersonnelList { get; set; }
        public List<PersonnelRoleDTO> PersonnelRoleList { get; set; }
    }
}
