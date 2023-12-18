using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiAgile.Shared.Personnel
{
    public class PersonnelDTO
    {
        public int RecordID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsActive { get; set; }
        public int PersonnelRoleID { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public string NameSurname { get { return (Name + " " + Surname).Trim(); } }
    }
}
