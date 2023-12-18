using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiAgile.Shared.User
{
    public class UserDTO
    {
        public int RecordID { get; set; }
        public string? Image { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Repassword { get; set; }
        public bool IsActive { get; set; }
        public int UserRoleID { get; set; }
        public string Username { get; set; }
    }
}
