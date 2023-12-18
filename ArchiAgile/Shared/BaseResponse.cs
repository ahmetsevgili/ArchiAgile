using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiAgile.Shared
{
    public class BaseResponse
    {
        public string ResponseCode { get; set; } = "";
        public string ResponseMessage { get; set; } = "";
    }
}
