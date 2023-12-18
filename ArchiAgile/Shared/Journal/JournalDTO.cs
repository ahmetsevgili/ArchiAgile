using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiAgile.Shared.Journal
{
    public class JournalDTO
    {
        public int RecordID { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int PersonnelID { get; set; }
        public string Note { get; set; }
        public int ConferPersonnelID { get; set; }
        public int ConferPersonnelRoleID { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
