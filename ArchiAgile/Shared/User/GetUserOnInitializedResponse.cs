using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiAgile.Shared.User
{
    public class GetUserOnInitializedResponse : BaseResponse
    {
        public List<UserDTO> UserList { get; set; }
        public List<UserRoleDTO> UserRoleList { get; set; }
    }
}
