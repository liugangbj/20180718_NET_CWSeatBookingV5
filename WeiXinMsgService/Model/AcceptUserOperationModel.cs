using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinMsgService.Model
{
   public class AcceptUserOperationModel
    {
        public string SchoolNum { get; set; }
        public string StudentNo { get; set; }
        public string MsgType { get; set; }
        public string Room { get; set; }
        public string SeatNo { get; set; }
        public string AddTime { get; set; }
        public string EndTime { get; set; }
        public string Days { get; set; }
        public string Msg { get; set; }
    }
}
