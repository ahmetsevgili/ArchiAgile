using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiAgile.Shared.User
{
    public class CurrentUserDTO
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public Dictionary<string, string> Claims { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int UserID { get; set; }
        public string Image { get; set; }
    }
}
