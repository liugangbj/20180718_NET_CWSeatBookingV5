using SeatManage.ClassModel;
using SeatManage.IWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WcfServiceForSeatManage;

namespace SeatManageWebV5.WebService
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string GetSeatLayoutListJson()
        {
            List<SeatLayout> list = new List<SeatLayout>();
            ISeatManageService seatManage = new WcfServiceForSeatManage.SeatManageDateService();
            List<ReadingRoomInfo> rooms = seatManage.GetReadingRoomInfo(null);
            foreach (var item in rooms)
            {
                SeatLayout layOut = seatManage.GetRoomSeatLayOut(item.No);
                list.Add(layOut);
            }
          string s =  Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return s;
        }
    }
}
