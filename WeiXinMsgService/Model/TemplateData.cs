using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinMsgService.Model
{
    public class TemplateData
    {
        public TempItem first { get; set; }
        public TempItem keyword1 { get; set; }
        public TempItem keyword2 { get; set; }
        public TempItem keyword3 { get; set; }
        public TempItem remark { get; set; }
    }

    public class TempItem
    {
        public TempItem(string v, string c = "#173177")
        {
            value = v;
            color = c;
        }
        public string value { get; set; }
        public string color { get; set; }
    }
}
