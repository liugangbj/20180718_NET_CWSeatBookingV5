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

        #region 进出记录查询
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
            DateTime startDate = string.IsNullOrEmpty(beginDateString) ? DateTime.Now.AddDays(-7).Date : DateTime.Parse(beginDateString);

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


        public ActionResult SelectEnterOutLog()
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
            ViewBag.Data = GetEnterOutGridString(null, null, null);
            ViewBag.RoomList = sb.ToString();
            return View();
        }
        #endregion

        #region 违规记录
        private DataTable GetUserInfoDateTable(string starttime, string endtime, string roomNo, string logstatus, string blacklist)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("CardNo", typeof(string));
            dt.Columns.Add("ReaderName", typeof(string));
            dt.Columns.Add("AddTime", typeof(string));
            dt.Columns.Add("ReadingRoom", typeof(string));
            dt.Columns.Add("Seat", typeof(string));
            dt.Columns.Add("LogStatus", typeof(string));
            dt.Columns.Add("BlacklistStatus", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            List<SeatManage.ClassModel.ViolationRecordsLogInfo> VRlist = SeatManage.Bll.T_SM_ViolateDiscipline.GetViolationRecords(
                this.LoginId,
                roomNo,
                starttime,
                endtime,
                (SeatManage.EnumType.LogStatus)int.Parse(logstatus),
                (SeatManage.EnumType.LogStatus)int.Parse(blacklist));
            foreach (SeatManage.ClassModel.ViolationRecordsLogInfo vrinfo in VRlist)
            {

                DataRow dr = dt.NewRow();
                dr["ID"] = vrinfo.ID;
                dr["CardNo"] = vrinfo.CardNo;
                dr["ReaderName"] = vrinfo.ReaderName;
                dr["AddTime"] = vrinfo.EnterOutTime;
                dr["ReadingRoom"] = vrinfo.ReadingRoomName;
                dr["Seat"] = vrinfo.SeatID;
                if (vrinfo.Flag == LogStatus.Valid)
                {
                    dr["LogStatus"] = "有效记录";
                }
                else
                {
                    dr["LogStatus"] = "失效记录";
                }
                if (vrinfo.BlacklistID != "-1")
                {
                    dr["BlacklistStatus"] = "已加入黑名单";
                }
                else
                {
                    dr["BlacklistStatus"] = "未处理";
                }
                dr["Remark"] = vrinfo.Remark;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public string ViolateDisciplineGridString(string beginDateString, string endDateString, string roomNoString, string selRecStatus, string selIsInBlack)
        {
            beginDateString = string.IsNullOrEmpty(beginDateString) ? DateTime.Now.AddDays(-7).ToShortDateString() : beginDateString;
            endDateString = string.IsNullOrEmpty(endDateString) ? DateTime.Now.ToShortDateString() : endDateString;
            selRecStatus = string.IsNullOrEmpty(selRecStatus) ? "-1" : selRecStatus;
            selIsInBlack = string.IsNullOrEmpty(selIsInBlack) ? "-1" : selIsInBlack;
            DataTable dt = GetUserInfoDateTable(beginDateString, DateTime.Parse(endDateString).AddHours(23).AddMinutes(59).ToString(), roomNoString, selRecStatus, selIsInBlack);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("{\"ID\": '" + r["ID"] + "',\"AddTime\": '" + r["AddTime"] + "',\"ReadingRoom\": \"" + r["ReadingRoom"] + "\",\"Seat\": \"" + r["Seat"] + "\",\"LogStatus\": \"" + r["LogStatus"] + "\",\"BlacklistStatus\": \"" + r["BlacklistStatus"] + "\",\"Remark\": \"" + r["Remark"] + "\"}");
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

        public ActionResult SelectViolateDiscipline()
        {
            string nowDay = DateTime.Now.ToShortDateString();
            string before7Day = DateTime.Now.AddDays(-7).ToShortDateString();
            ViewBag.nowDay = nowDay;
            ViewBag.before7Day = before7Day;
            //绑定阅览室下拉列表
            List<SeatManage.ClassModel.ReadingRoomInfo> roomlist = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);
            roomlist.Insert(0, new SeatManage.ClassModel.ReadingRoomInfo() { Name = "所有阅览室", No = "" });
            StringBuilder sbroomList = new StringBuilder();
            List<SeatManage.ClassModel.ReadingRoomInfo> roomList = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);
            if (roomList.Count > 0)
            {
                sbroomList.Append("<select  id=\"selRooms\" name=\"selRooms\">");
                sbroomList.Append("<option value=\"\">所有阅览室</option>");
                foreach (var item in roomList)
                {
                    sbroomList.Append("<option value=\"" + item.No + "\">" + item.Name + "</option>");
                }
                sbroomList.Append("</select>");
            }
            ViewBag.Data = ViolateDisciplineGridString(null, null, string.Empty, "-1", "-1");
            ViewBag.RoomList = sbroomList.ToString();
            return View();
        }
        #endregion


        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        private DataTable GetBlackListUserInfoDateTable(string starttime, string endtime,string selRecStatus)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("CardNo", typeof(string));
            dt.Columns.Add("ReaderName", typeof(string));
            dt.Columns.Add("AddTime", typeof(string));
            dt.Columns.Add("LeaveTime", typeof(string));
            dt.Columns.Add("LogStatus", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            List<SeatManage.ClassModel.BlackListInfo> Blistlistlist = SeatManage.Bll.T_SM_Blacklist.GetAllBlackListInfo(
                this.LoginId,
                (SeatManage.EnumType.LogStatus)int.Parse(selRecStatus),
                starttime,
                endtime);
            foreach (SeatManage.ClassModel.BlackListInfo bllist in Blistlistlist)
            {

                DataRow dr = dt.NewRow();
                dr["ID"] = bllist.ID;
                dr["CardNo"] = bllist.CardNo;
                dr["ReaderName"] = bllist.ReaderName;
                dr["AddTime"] = bllist.AddTime;
                dr["LeaveTime"] = bllist.OutTime;
                if (bllist.BlacklistState == LogStatus.Valid)
                {
                    dr["LogStatus"] = "处罚中";
                }
                else
                {
                    dr["LogStatus"] = "已过期";
                }
                dr["Remark"] = bllist.ReMark;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public string GetBlackListString(string beginDateString,string endDateString,string selRecStatus)
        {
            beginDateString = string.IsNullOrEmpty(beginDateString) ? DateTime.Now.AddDays(-7).ToShortDateString() : beginDateString;
            endDateString = string.IsNullOrEmpty(endDateString) ? DateTime.Now.ToShortDateString() : endDateString;
            selRecStatus = string.IsNullOrEmpty(selRecStatus) ? "-1" : selRecStatus;
            DataTable dt = GetBlackListUserInfoDateTable(beginDateString, string.Format("{0} {1}", endDateString, "23:59:59"), selRecStatus);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("{\"ID\": '" + r["ID"] + "',\"AddTime\": '" + r["AddTime"] + "',\"LeaveTime\": \"" + r["LeaveTime"] + "\",\"LogStatus\": \"" + r["LogStatus"] + "\",\"Remark\": \"" + r["Remark"] + "\"}");
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

        public ActionResult SelectBlacklist()
        {
            string nowDay = DateTime.Now.ToShortDateString();
            string before7Day = DateTime.Now.AddDays(-7).ToShortDateString();
            ViewBag.nowDay = nowDay;
            ViewBag.before7Day = before7Day;
            ViewBag.Data = GetBlackListString(null, null, string.Empty);
            return View();
        }
        public ActionResult SelectNoticeLog()
        {
            DataTable dt = LogQueryHelper.ReaderNoticeList(this.LoginId);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("{\"NoticeId\": '" + r["NoticeId"] + "',\"AddTime\": '" + r["AddTime"] + "',\"NoticeContent\": \"" + r["NoticeContent"] + "\"}");
                sb.Append(",");
            }
            if (dt.Rows.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            sb.Append("}");

            ViewBag.Data = sb.ToString();
            return View();
        }



    }
}