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

        #region 预约记录查询
        public JsonResult BespeakLogRemove(string BespeakID)
        {
            JsonResult result = null;
            int id = int.Parse(BespeakID);
            SeatManage.ClassModel.BespeakLogInfo bespeakModel = SeatManage.Bll.T_SM_SeatBespeak.GetBespeaklogById(id);
            if (bespeakModel.BsepeakState != BookingStatus.Waiting)
            {
                result = Json(new { status = "no", message = "只能删除状态为[等待确认]的预约记录，删除失败" }, JsonRequestBehavior.AllowGet);
            }
            bespeakModel.BsepeakState = BookingStatus.Cencaled;
            bespeakModel.CancelPerson = Operation.Reader;
            bespeakModel.CancelTime = SeatManage.Bll.ServiceDateTime.Now;
            bespeakModel.Remark = "读者取消预约";
            if (SeatManage.Bll.T_SM_SeatBespeak.UpdateBespeakList(bespeakModel) > 0)
            {
                result = Json(new { status = "yes", message = "预约取消成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = Json(new { status = "no", message = "操作失败" }, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        public string GetGridString(string beginDateString, string endDateString, string statusString)
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
            DateTime startDate = string.IsNullOrEmpty(beginDateString) ? DateTime.Now.Date : DateTime.Parse(beginDateString);
            DateTime endDate = string.IsNullOrEmpty(endDateString) ? DateTime.Now.AddDays(7).Date : DateTime.Parse(endDateString);//dpEndDate.SelectedDate.Value;
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
                sb.Append("{\"BespeakID\": '" + r["BespeakID"] + "',\"ReadingRoomName\": '" + r["ReadingRoomName"] + "',\"SeatNum\": \"" + r["SeatNum"] + "\",\"BsepeakState\": \"" + r["BsepeakState"] + "\",\"SubmitTime\": \"" + r["SubmitTime"] + "\",\"BespeakTime\": \"" + r["BespeakTime"] + "\",\"CancelTime\": \"" + r["CancelTime"] + "\",\"Remark\": \"" + r["Remark"] + "\"}");
                sb.Append(",");
            }
            if (dt.Rows.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

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
                sb.Append("<option value=\"" + item.Value + "\">" + item.BespeakState + "</option>");
            }
            sb.Append("</select>");
            ViewBag.Data = GetGridString(beginDateString, endDateString, statusString);
            ViewBag.BespeakEnumKeyValue = sb.ToString();
            return View();
        }
        #endregion

        /// <summary>
        /// 进出记录查询
        /// </summary>
        /// <param name="beginDateString"></param>
        /// <param name="endDateString"></param>
        /// <param name="roomNo"></param>
        /// <returns></returns>
        public string GetEnterOutGridString(string beginDateString, string endDateString, string roomNo)
        {
            DateTime serviceDate = SeatManage.Bll.ServiceDateTime.Now;
            string cardNo = this.LoginId;
            DateTime startDate = string.IsNullOrEmpty(beginDateString)?DateTime.Now.AddDays(-7).Date: DateTime.Parse(beginDateString);
            StringBuilder sb = new StringBuilder();
            if (startDate.Date < SeatManage.Bll.ServiceDateTime.Now.AddDays(-30).Date)
            {
                return "-1";
            }
            else
            {
                DateTime endDate = string.IsNullOrEmpty(endDateString) ? DateTime.Now.Date : DateTime.Parse(endDateString); //DateTime.Parse(string.Format("{0} {1}", dpEndDate.Text, " 23:59:59"));
                EnumEnterOutLogQueryMethod method = EnumEnterOutLogQueryMethod.CardNo;
                DataTable dt = null;
                dt = LogQueryHelper.GetEnterOutLogDataSet(this.LoginId, roomNo, method, startDate, endDate.AddHours(23).AddMinutes(59));
                sb.Append("{");
                sb.Append("\"form.paginate.pageNo\": 1,");
                sb.Append("\"form.paginate.totalRows\": 100,");
                sb.Append("	\"rows\": [");
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("{\"ReadingRoomName\": '" + r["ReadingRoomName"] + "',\"SeatShortNum\": '" + r["SeatShortNum"] + "',\"Status\": \"" + r["Status"] + "\",\"EnterOutTime\": \"" + r["EnterOutTime"] + "\",\"Remark\": \"" + r["Remark"] + "\"}");
                    sb.Append(",");
                }
                if (dt.Rows.Count > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
                sb.Append("}");
            }
            return sb.ToString();
        }


        public ActionResult SelectEnterOutLog(string beginDateString, string endDateString, string roomNoString)
        {
            string nowDay = DateTime.Now.ToShortDateString();
            string before7Day = DateTime.Now.AddDays(-7).ToShortDateString();
            ViewBag.nowDay = nowDay;
            ViewBag.before7Day = before7Day;
            StringBuilder sb = new StringBuilder();
            List<SeatManage.ClassModel.ReadingRoomInfo> roomList = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);
            if (roomList.Count > 0)
            {
                sb.Append("<select  id=\"selRooms\" name=\"selRooms\">");
                sb.Append("<option value=\"\">所有阅览室</option>");
                foreach (var item in roomList)
                {
                    sb.Append("<option value=\"" + item.No + "\">" + item.Name + "</option>");
                }
                sb.Append("</select>");
            }
            ViewBag.Data = GetEnterOutGridString(beginDateString, endDateString, roomNoString);
            ViewBag.RoomList = sb.ToString();
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