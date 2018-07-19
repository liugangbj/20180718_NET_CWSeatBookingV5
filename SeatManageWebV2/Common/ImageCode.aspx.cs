using SeatManageWebV5.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeatManageWebV5.Common
{
    public partial class ImageCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            VCode v = new VCode();
            string code = v.CreateVerifyCode();            //取随机码    
            v.CreateImageOnPage(code, this.Context);       // 输出图片    
            Session["CheckCode"] = code;
        }


        public  void ReLoadCode()
        {
            Page_Load(null, null);
            //Response.AddHeader("Refresh", "0");
        }
    }
}