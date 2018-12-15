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
    public class LogManageController : BaseController
    {
        // GET: LogManage
        public ActionResult Index()
        {
            return View();
        }

        public string GetBespeakStateData()
        {
            List<BespeakEnumKey_Value> list = new List<BespeakEnumKey_Value>();
            list.Add(new BespeakEnumKey_Value() { BespeakState = SeatManage.SeatManageComm.SeatComm.ConvertBookingStatus(SeatManage.EnumType.BookingStatus.None), Value = ((int)SeatManage.EnumType.BookingStatus.None).ToString() });

            list.Add(new BespeakEnumKey_Value() { BespeakState = SeatManage.SeatManageComm.SeatComm.ConvertBookingStatus(SeatManage.EnumType.BookingStatus.Waiting), Value = ((int)SeatManage.EnumType.BookingStatus.Waiting).ToString() });
            list.Add(new BespeakEnumKey_Value() { BespeakState = SeatManage.SeatManageComm.SeatComm.ConvertBookingStatus(SeatManage.EnumType.BookingStatus.Confinmed), Value = ((int)SeatManage.EnumType.BookingStatus.Confinmed).ToString() });
            list.Add(new BespeakEnumKey_Value() { BespeakState = SeatManage.SeatManageComm.SeatComm.ConvertBookingStatus(SeatManage.EnumType.BookingStatus.Cencaled), Value = ((int)SeatManage.EnumType.BookingStatus.Cencaled).ToString() });
            StringBuilder sb = new StringBuilder();
            if (list != null)
            {
                sb.Append("{\"list\":[");
                foreach (var item in list)
                {
                    sb.Append("{\"key\":\"" + item.BespeakState + "\",\"value\":\"" + item.Value + "\"},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 绑定房间列表
        /// </summary>
        /// <returns></returns>
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


        public string GetBespeakData()
        {
            string result = "";
            StringBuilder sb = new StringBuilder();

            string cardNo = Request["txtNum"].ToString().Trim();
            string roomNum = Request.Params["selectRooms"]; //ddlReadingRoom.SelectedItem.Value;
            BookingStatus status = (BookingStatus)int.Parse(Request.Params["selectBespeakState"].ToString());
            DateTime startDate = DateTime.Parse(Request.Params["beginDate"].ToString());
            DateTime endDate = DateTime.Parse(Request.Params["endDate"].ToString());
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
            DataTable dt = new DataTable();
            //if (chkSearchMH.Checked == false)
            if(Request.Params["chkSearchMH"]==null)
            {
                dt = LogQueryHelper.BespeakLogQuery(cardNo, roomNum, statusList, startDate, endDate.AddHours(23).AddMinutes(59));
            }
            else
            {
                dt = LogQueryHelper.BespeakLogQuery_ByFuzzySearch(cardNo, roomNum, statusList, startDate, endDate.AddHours(23).AddMinutes(59));
            }
            if (dt != null)
            {
                sb.Append("{");
                sb.Append("\"form.paginate.pageNo\": 1,");
                sb.Append("\"form.paginate.totalRows\": 100,");
                sb.Append("	\"rows\": [");
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("{\"BespeakID\": '" + r["BespeakID"] + "',\"CardNo\": '" + r["CardNo"] + "',\"ReaderName\": \"" + r["ReaderName"] + "\",\"ReadingRoomName\": \"" + r["ReadingRoomName"] + "\",\"SeatNum\": \"" + r["SeatNum"] + "\",\"BsepeakState\": \"" + r["BsepeakState"] + "\",\"SubmitTime\": \"" + r["SubmitTime"] + "\",\"BespeakTime\": \"" + r["BespeakTime"] + "\",\"CancelTime\": \"" + r["CancelTime"] + "\",\"Remark\": \"" + r["Remark"] + "\"}");
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
            return result;
        }

        public ActionResult BespeakLog()
        {
            string nowDay = DateTime.Now.ToShortDateString();
            ViewBag.nowDay = nowDay;
            return View();
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        private DataTable GetUserInfoDateTable(string starttime, string endtime,bool chkSearchMH,string txtNum,
            string ddlReadingRoom,string ddllogstatus,string ddlblacklist,string ddlVrType)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("CardNo", typeof(string));
            dt.Columns.Add("ReaderName", typeof(string));
            dt.Columns.Add("AddTime", typeof(DateTime));
            dt.Columns.Add("ReadingRoom", typeof(string));
            dt.Columns.Add("Seat", typeof(string));
            dt.Columns.Add("LogStatus", typeof(string));
            dt.Columns.Add("BlacklistStatus", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            List<SeatManage.ClassModel.ViolationRecordsLogInfo> VRlist = new List<SeatManage.ClassModel.ViolationRecordsLogInfo>();
            if (chkSearchMH == false)
            {
                VRlist = SeatManage.Bll.T_SM_ViolateDiscipline.GetViolationRecords(
                txtNum,
                ddlReadingRoom,
                starttime,
                endtime,
                (SeatManage.EnumType.LogStatus)int.Parse(ddllogstatus),
                (SeatManage.EnumType.LogStatus)int.Parse(ddlblacklist),
                (SeatManage.EnumType.ViolationRecordsType)int.Parse(ddlVrType));
            }
            else
            {
                VRlist = SeatManage.Bll.T_SM_ViolateDiscipline.GetViolationRecords_ByFuzzySearch(
                    txtNum,
                    ddlReadingRoom,
                    starttime,
                    endtime,
                    (SeatManage.EnumType.LogStatus)int.Parse(ddllogstatus),
                    (SeatManage.EnumType.LogStatus)int.Parse(ddlblacklist),
                    (SeatManage.EnumType.ViolationRecordsType)int.Parse(ddlVrType));
            }

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

        public string GetViolateData()
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            DateTime starttime = DateTime.Parse(Request.Params["beginDate"]);
            DateTime endtime = DateTime.Parse(Request.Params["endDate"]);

            if (starttime >= endtime)
            {
                result = "结束日期必须大于等于开始日期";
            }
            else
            {
                bool chkSearchMH = Request.Params["chkSearchMH"] == null ? false : true;
                string txtNum = Request.Params["txtNum"].Trim();
                string ddlReadingRoom = Request.Params["selectRooms"];
                string ddllogstatus = Request.Params["selectlogstatus"];
                string ddlblacklist = Request.Params["selectblacklist"];
                string ddlVrType = Request.Params["selectVrType"];
                DataTable dt = GetUserInfoDateTable(starttime.ToString(), endtime.ToString(), chkSearchMH,txtNum,ddlReadingRoom,ddllogstatus,ddlblacklist,ddlVrType);

                sb.Append("{");
                sb.Append("\"form.paginate.pageNo\": 1,");
                sb.Append("\"form.paginate.totalRows\": 100,");
                sb.Append("	\"rows\": [");
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("{\"ID\": '" + r["ID"] + "',\"CardNo\": '" + r["CardNo"] + "',\"ReaderName\": \"" + r["ReaderName"] + "\",\"AddTime\": \"" + r["AddTime"] + "\",\"ReadingRoom\": \"" + r["ReadingRoom"] + "\",\"Seat\": \"" + r["Seat"] + "\",\"LogStatus\": \"" + r["LogStatus"] + "\",\"BlacklistStatus\": \"" + r["BlacklistStatus"] + "\",\"Remark\": \"" + r["Remark"] + "\"}");
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
            return result;
        }


        public ActionResult ViolateDiscipline()
        {
            string nowDay = DateTime.Now.ToShortDateString();
            string before7Day = DateTime.Now.AddDays(-7).ToShortDateString();
            ViewBag.nowDay = nowDay;
            ViewBag.before7Day = before7Day;
            return View();
        }
        public ActionResult Blacklist()
        {
            return View();
        }
    }
}