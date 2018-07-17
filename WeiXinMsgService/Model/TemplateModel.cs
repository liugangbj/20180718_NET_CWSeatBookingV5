using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinMsgService.Model
{
   public class TemplateModel
    {
        public string touser { get; set; }
        public string template_id { get; set; }
        public string url { get; set; }

        public string topcolor { get; set; }

        public TemplateData data { get; set; }

        public TemplateModel(string first, string keyword1, string keyword2, string keyword3, string remark)
        {
            data = new TemplateData()
            {
                first = new TempItem(first),
                keyword1 = new TempItem(keyword1),
                keyword2 = new TempItem(keyword2),
                keyword3 = new TempItem(keyword3),
                remark = new TempItem(remark)
            };

        }
    }
}
