using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeatManageWebV2.FunctionPages.SeatBespeak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SeatManageWebV2.WebService
{
    /// <summary>
    /// Summary description for SeatLayoutInterface
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SeatLayoutInterface : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        static SeatLayoutHandle seatLayout = new SeatLayoutHandle();

        public string GetLayOutHtml(string schoolNo,string StudentNo,string Date,string RoomNo)
        {
            return "";
        }

    }
}
