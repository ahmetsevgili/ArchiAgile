using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiAgile.Shared.Journal
{
    public class GetJournalPaginationResponse : BaseResponse
    {
        public int Count { get; set; }
        public List<JournalDTO> JournalList { get; set; }
    }
}
