using SeatManage.EnumType;
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
    public class ReaderLogController : BaseController
    {
        // GET: ReaderLog
        public ActionResult Index()
        {
            return View();
        }

        public string GetGridString(string beginDateString,string endDateString,string statusString)
        {
            string cardNo = this.LoginId;
            if (string.IsNullOrEmpty(cardNo))
            {
                cardNo = this.LoginId;
            }
            BookingStatus status = BookingStatus.None;
            if (statusString != null)
            {
                status = (BookingStatus)int.Parse(statusString);
            }
            DateTime startDate = string.IsNullOrEmpty(beginDateString)? DateTime.Now.Date: DateTime.Parse(beginDateString);
            DateTime endDate = string.IsNullOrEmpty(endDateString) ? DateTime.Now.AddDays(7).Date : DateTime.Parse(endDateString); ;//dpEndDate.SelectedDate.Value;
            List<BookingStatus> statusList = new List<BookingStatus>();
            if (status == BookingStatus.None)
            {
                statusList.Add(BookingStatus.Cencaled);
                statusList.Add(BookingStatus.Confinmed);
                statusList.Add(BookingStatus.Waiting);
            }
            else
            {
                statusList.Add(status);
            }
            DataTable dt = LogQueryHelper.BespeakLogQuery(cardNo, null, statusList, startDate, endDate.AddHours(23).AddMinutes(59));
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("{\"ReadingRoomName\": '" + r["ReadingRoomName"] + "',\"SeatNum\": \"" + r["SeatNum"] + "\",\"BsepeakState\": \"" + r["BsepeakState"] + "\",\"SubmitTime\": \"" + r["SubmitTime"] + "\",\"BespeakTime\": \"" + r["BespeakTime"] + "\",\"CancelTime\": \"" + r["CancelTime"] + "\",\"Remark\": \"" + r["Remark"] + "\"}");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append("}");

            return sb.ToString();
        }

        public ActionResult SelectBespeakLog(string beginDateString, string endDateString, string statusString)
        {
            string nowDay = DateTime.Now.ToShortDateString();
            string after7Day = DateTime.Now.AddDays(7).ToShortDateString();
            ViewBag.nowDay = nowDay;
            ViewBag.after7Day = after7Day;

            List<BespeakEnumKey_Value> list = new List<BespeakEnumKey_Value>();
            list.Add(new BespeakEnumKey_Value() { BespeakState = SeatManage.SeatManageComm.SeatComm.ConvertBookingStatus(SeatManage.EnumType.BookingStatus.None), Value = ((int)SeatManage.EnumType.BookingStatus.None).ToString() });
            list.Add(new BespeakEnumKey_Value() { BespeakState = SeatManage.SeatManageComm.SeatComm.ConvertBookingStatus(SeatManage.EnumType.BookingStatus.Waiting), Value = ((int)SeatManage.EnumType.BookingStatus.Waiting).ToString() });
            list.Add(new BespeakEnumKey_Value() { BespeakState = SeatManage.SeatManageComm.SeatComm.ConvertBookingStatus(SeatManage.EnumType.BookingStatus.Confinmed), Value = ((int)SeatManage.EnumType.BookingStatus.Confinmed).ToString() });
            list.Add(new BespeakEnumKey_Value() { BespeakState = SeatManage.SeatManageComm.SeatComm.ConvertBookingStatus(SeatManage.EnumType.BookingStatus.Cencaled), Value = ((int)SeatManage.EnumType.BookingStatus.Cencaled).ToString() });

            StringBuilder sb = new StringBuilder();

            sb.Append("<select prompt=\"预约状态\"  id=\"selStatus\" name=\"selStatus\">");

            foreach (var item in list)
            {
                sb.Append("<option value=\""+item.Value+"\">"+item.BespeakState+"</option>");
            }
            sb.Append("</select>");
            ViewBag.Data = GetGridString(beginDateString, endDateString, statusString);
            ViewBag.BespeakEnumKeyValue = sb.ToString();
            return View();
        }



        public ActionResult SelectEnterOutLog()
        {
            return View();
        }

        public ActionResult SelectViolateDiscipline()
        {
            return View();
        }

        public ActionResult SelectBlacklist()
        {
            return View();
        }
        public ActionResult SelectNoticeLog()
        {
            return View();
        }



    }
}