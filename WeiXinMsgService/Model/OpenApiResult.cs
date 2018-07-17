using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinMsgService.Model
{
    public class OpenApiResult
    {
        public int error_code { get; set; }
        public string error_msg { get; set; }

        public string msg_id { get; set; }
    }
}
