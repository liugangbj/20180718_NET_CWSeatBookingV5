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
    public class LogManageController : BaseController
    {
        // GET: LogManage
        public ActionResult Index()
        {
            return View();
        }

        public string GetRoomData()
        {
            SeatManage.ClassModel.ManagerPotency potency = SeatManage.Bll.T_SM_ManagerPotency.GetManangePotencyByLoginID(this.LoginId);
            StringBuilder sb = new StringBuilder();
            if (potency != null)
            {
                sb.Append("{\"list\":[");
                sb.Append("{\"key\":\"所有阅览室\",\"value\":\"\"},");
                foreach (var item in potency.RightRoomList)
                {
                    sb.Append("{\"key\":\""+item.Name+"\",\"value\":\""+item.No+"\"},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]}");
            }
            return sb.ToString();
        }


        public string GetEnterOutData()
        {
            string result = "";

            string chkSearchMH = Request.Params["chkSearchMH"] == null ? string.Empty : Request.Params["chkSearchMH"];
            string num = Request.Params["txtNum"].Trim();
            string roomNum = Request.Params["selectRooms"].Trim();
            DateTime startDate = DateTime.Parse(Request.Params["beginDate"].Trim());
            StringBuilder sb = new StringBuilder();
            if (startDate.Date < SeatManage.Bll.ServiceDateTime.Now.AddDays(-30).Date)
            {
                result = "最多可以查询30天前的数据";
            }
            else
            {
                DateTime endDate = DateTime.Parse(Request.Params["endDate"].Trim());
                EnumEnterOutLogQueryMethod method = EnumEnterOutLogQueryMethod.None;
                DataTable dt = null;
                if (Request.Params["selectCardNoOrSeatNo"] == "cardNo")
                {
                    method = EnumEnterOutLogQueryMethod.CardNo;
                }
                else if (Request.Params["selectCardNoOrSeatNo"] == "seatNum")
                {
                    method = EnumEnterOutLogQueryMethod.SeatNum;
                }
                if (chkSearchMH == string.Empty)
                {
                    dt = LogQueryHelper.GetEnterOutLogDataSet(num, roomNum, method, startDate, endDate.AddHours(23).AddMinutes(59));
                }
                else
                {
                    dt = LogQueryHelper.GetEnterOutLogDataSet_ByFuzzySearch(num, roomNum, method, startDate, endDate.AddHours(23).AddMinutes(59));
                }
                if (dt != null)
                {
                    sb.Append("{");
                    sb.Append("\"form.paginate.pageNo\": 1,");
                    sb.Append("\"form.paginate.totalRows\": 100,");
                    sb.Append("	\"rows\": [");
                    foreach (DataRow r in dt.Rows)
                    {
                        sb.Append("{\"CardNo\": '" + r["CardNo"] + "',\"ReaderName\": '" + r["ReaderName"] + "',\"ReadingRoomName\": \"" + r["ReadingRoomName"] + "\",\"SeatShortNum\": \"" + r["SeatShortNum"] + "\",\"Status\": \"" + r["Status"] + "\",\"EnterOutTime\": \"" + r["EnterOutTime"] + "\",\"Remark\": \"" + r["Remark"] + "\"}");
                        sb.Append(",");
                    }
                    if (dt.Rows.Count > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }
                    sb.Append("]");
                    sb.Append("}");
                    result = sb.ToString();
                }
                else
                {
                    result = "没有查询到信息";
                }
            }

            return result;
        }

        public ActionResult EnterOutLog()
        {
            string nowDay = DateTime.Now.ToShortDateString();
            string before1Day = DateTime.Now.AddDays(-1).ToShortDateString();
            ViewBag.nowDay = nowDay;
            ViewBag.before1Day = before1Day;
            return View();
        }
        public ActionResult BespeakLog()
        {
            return View();
        }
        public ActionResult ViolateDiscipline()
        {
            return View();
        }
        public ActionResult Blacklist()
        {
            return View();
        }
    }
}