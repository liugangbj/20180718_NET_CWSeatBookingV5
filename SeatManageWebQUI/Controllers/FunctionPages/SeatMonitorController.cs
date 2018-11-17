using SeatManageWebV5.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{

    public class SeatMonitorController : BaseController
    {
        // GET: SeatMonitor
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult SeatHandle(string seatNo,string seatShortNo,string used)
        {

            return View();
        }

        public string DrowSeatLayoutHtml(string roomNum, string divTransparentTop, string divTransparentLeft)
        {
            string html = "";
            Code.SeatLayoutTools tool = new Code.SeatLayoutTools();
            html = tool.drowSeatLayoutHtml(roomNum, divTransparentTop, divTransparentLeft);
            return html;
        }

        public ActionResult SeatGraph(string roomId)
        {
            ViewBag.roomId = roomId;
            return View();
        }

        private string MonitorGraphModeDataBind()
        {
            string result = "";
            DataTable dt = LogQueryHelper.GetMonitorGraphReadingRoomList(this.LoginId);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (DataRow item in dt.Rows)
            {
                sb.Append("{\"roomNum\": " + item["roomNum"] + ",\"roomName\": \"" + item["roomName"] + "\",\"libraryName\": \"" + item["libraryName"] + "\",\"seatCountAll\": \"" + item["seatCountAll"] + "\",\"seatCountUsed\": \"" + item["seatCountUsed"] + "\",\"seatCountShortLeave\": \"" + item["seatCountShortLeave"] + "\"}");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append("}");
            string data = sb.ToString();
            ViewBag.Data = data;
            return result;
        }

        public ActionResult MonitorGraphMode()
        {
            MonitorGraphModeDataBind();
            return View();
        }
        public ActionResult DeviceStatusInfo()
        {
            return View();
        }
    }
}