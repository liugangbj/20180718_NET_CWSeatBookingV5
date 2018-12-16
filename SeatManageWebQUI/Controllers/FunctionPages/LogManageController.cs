using SeatManage.Bll;
using SeatManage.ClassModel;
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

        public JsonResult BlackRemoveAll(string IDs)
        {
            JsonResult result = null;
            string[] arr = IDs.Split(',');
            List<ViolationRecordsLogInfo> list = new List<ViolationRecordsLogInfo>();
            int count = 0;
            foreach (var item in arr)
            {
                SeatManage.ClassModel.BlackListInfo blacklist = SeatManage.Bll.T_SM_Blacklist.GetBlistList(item);
                if (item != null)
                {
                    blacklist.BlacklistState = LogStatus.Fail;
                    if (SeatManage.Bll.T_SM_Blacklist.UpdateBlackList(blacklist)>0)
                    {
                        count++;
                    }
                }
                else
                {
                    continue;
                }
            }
            result = Json(new { status = "yes", message = "成功删除" + count + "条记录" }, JsonRequestBehavior.AllowGet);
            return result;
        }

        public JsonResult BlackRemove(string ID)
        {
            JsonResult result = null;
            SeatManage.ClassModel.BlackListInfo blacklist = SeatManage.Bll.T_SM_Blacklist.GetBlistList(ID);
            if (blacklist != null)
            {
                blacklist.BlacklistState = LogStatus.Fail;
                if (SeatManage.Bll.T_SM_Blacklist.UpdateBlackList(blacklist) == 0)
                {
                    result = Json(new { status = "no", message = "移除失败" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //SeatManage.ClassModel.ReaderNoticeInfo rni = new SeatManage.ClassModel.ReaderNoticeInfo();
                    //rni.CardNo = blacklist.CardNo;
                    //rni.Type = NoticeType.DeleteBlacklistWarning;
                    //rni.Note = "被管理员手动移除黑名单";
                    //if (SeatManage.Bll.T_SM_ReaderNotice.AddReaderNotice(rni) > 0)
                    //{
                    result = Json(new { status = "yes", message = "移除成功" }, JsonRequestBehavior.AllowGet);
                    //}
                    //else
                    //{
                    //    FineUI.Alert.ShowInTop("添加消息失败！");
                    //}
                }
            }
            else
            {
                result = Json(new { status = "no", message = "移除失败" }, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        public ActionResult AddNewBlack()
        {
           string date = DateTime.Now.AddDays(7).ToString("yyyy/MM/dd");
            ViewBag.dpEndDate = date;
            return View();
        }

        public JsonResult SaveNewBlack()
        {
            JsonResult result = null;
            SeatManage.ClassModel.RegulationRulesSetting regulationRulesSetting = SeatManage.Bll.T_SM_SystemSet.GetRegulationRulesSetting();
            string CardNo = Request.Params["txtNum"].Trim();
            string Remark = Request.Params["txtRemark"].Trim(); //txtRemark.Text;
            string ReadingRoomNo = Request.Params["selectRooms"].ToString();//ddlroom.SelectedValue;
            SeatManage.ClassModel.ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(ReadingRoomNo);
            SeatManage.ClassModel.BlackListInfo bli = new SeatManage.ClassModel.BlackListInfo();
            bli.CardNo = CardNo;
            bli.AddTime = SeatManage.Bll.ServiceDateTime.Now;
            if(Request.Params["ddlleaveMode"] =="0") //(ddlleaveMode.SelectedValue == "0")
            {
                if ( DateTime.Parse(Request.Params["dpEndDate"]) < bli.AddTime.Date)
                {
                    result = Json(new { status = "no", message = "请输入不小于今天的日期" }, JsonRequestBehavior.AllowGet);
                    //FineUI.Alert.Show("请输入不小于今天的日期！");
                    return result;
                }
                bli.OutTime = DateTime.Parse(Request.Params["dpEndDate"] + " 23:59:59");
            }
            bli.OutBlacklistMode = (SeatManage.EnumType.LeaveBlacklistMode)int.Parse(Request.Params["ddlleaveMode"]);
            if (bli.OutBlacklistMode == SeatManage.EnumType.LeaveBlacklistMode.ManuallyMode)
            {
                bli.ReMark = string.Format("被管理员{0}加入手动加入黑名单，管理员手动移除黑名单，备注：{1}", this.LoginId, Remark);
            }
            else
            {
                bli.ReMark = string.Format("被管理员{0}加入手动加入黑名单，记录黑名单{1}天，备注：{2}", this.LoginId, (bli.OutTime - bli.AddTime).Days, Remark);
            }
            bli.ReadingRoomID = ReadingRoomNo;
            int blackId = 0;
            bool cbIsAllRR = Request.Params["cbIsAllRR"] == null ? false : true;
            if (cbIsAllRR)
            {
                int roomCount = 0;
                List<SeatManage.ClassModel.ReadingRoomInfo> roomlist = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);
                foreach (SeatManage.ClassModel.ReadingRoomInfo roominfo in roomlist)
                {
                    if (roominfo.Setting.BlackListSetting.Used)
                    {
                        bli.ReadingRoomID = roominfo.No;
                        if (!(SeatManage.Bll.T_SM_Blacklist.AddBlackList(bli) > 0))
                        {
                            result = Json(new { status = "no", message = "添加失败" }, JsonRequestBehavior.AllowGet);
                          //  FineUI.Alert.Show("添加失败！");
                           // return;
                        }
                        else
                        {
                            roomCount++;
                        }
                    }
                }
                if (roomCount == 0)
                {
                    blackId = SeatManage.Bll.T_SM_Blacklist.AddBlackList(bli);
                }
                result = Json(new { status = "yes", message = "添加成功" }, JsonRequestBehavior.AllowGet);
                //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                //FineUI.Alert.Show("添加成功！");
            }
            else
            {
                blackId = SeatManage.Bll.T_SM_Blacklist.AddBlackList(bli);
                if (blackId > 0)
                {
                    result = Json(new { status = "yes", message = "添加成功" }, JsonRequestBehavior.AllowGet);
                   // PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                  //  FineUI.Alert.Show("添加成功！");
                }
                else
                {
                    result = Json(new { status = "no", message = "添加失败" }, JsonRequestBehavior.AllowGet);
                    //FineUI.Alert.Show("添加失败！");
                }
            }

            return result;
        }


        public ActionResult AddNewViolate()
        {
            return View();
        }

        public JsonResult SaveNewViolate()
        {
            JsonResult result = null;
            SeatManage.ClassModel.RegulationRulesSetting regulationRulesSetting = T_SM_SystemSet.GetRegulationRulesSetting();
            string CardNo = Request.Params["txtNum"].Trim();
            string SeatNo = Request.Params["txtSeat"].Trim();
            string seatnoremark = "";
            if (!string.IsNullOrEmpty(SeatNo))
            {
                seatnoremark = SeatNo + "号座位，";
            }
            string Remark = Request.Params["txtRemark"].Trim();// txtRemark.Text;
            ViolationRecordsType Type = (ViolationRecordsType)int.Parse(Request.Params["selectVrType"].Trim());
            string ReadingRoomNo = Request.Params["selectRooms"].Trim();//ddlroom.SelectedValue;
            ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(ReadingRoomNo);
            if (room.Setting.IsRecordViolate)
            {
                ViolationRecordsLogInfo vrli = new ViolationRecordsLogInfo();
                vrli.CardNo = CardNo;
                vrli.SeatID = SeatNo;
                vrli.ReadingRoomID = ReadingRoomNo;
                vrli.EnterOutTime = ServiceDateTime.Now.ToString();
                vrli.EnterFlag = Type;
                vrli.Remark = string.Format("在{0}，{1}被管理员{2}，手动记录违规，备注{3}", room.Name, seatnoremark, this.LoginId, Remark);
                if (T_SM_ViolateDiscipline.AddViolationRecords(vrli))
                {
                    result = Json(new { status = "yes", message = "添加成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = Json(new { status = "no", message = "添加失败" }, JsonRequestBehavior.AllowGet);
                  //  FineUI.Alert.Show("添加失败！");
                }
            }
            return result;
        }

        public JsonResult ViolateRemoveAll(string ViolateIDs)
        {
            JsonResult result = null;
            string[] arrViolateIDS = ViolateIDs.Split(',');
            List<ViolationRecordsLogInfo> list = new List<ViolationRecordsLogInfo>();
            int count = 0;
            foreach (var item in arrViolateIDS)
            {
                SeatManage.ClassModel.ViolationRecordsLogInfo vrinfo = SeatManage.Bll.T_SM_ViolateDiscipline.GetViolationRecords(item);
                if (vrinfo != null)
                {
                    vrinfo.Flag = LogStatus.Fail;
                    if (SeatManage.Bll.T_SM_ViolateDiscipline.UpdateViolationRecords(vrinfo))
                    {
                        count++;
                    }
                }
                else
                {
                    continue;
                }
            }
            result = Json(new { status = "yes", message = "成功删除"+count+"条记录" }, JsonRequestBehavior.AllowGet);
            return result;
        }

        public JsonResult ViolateRemove(string ViolateID)
        {
            JsonResult result = null;
            SeatManage.ClassModel.ViolationRecordsLogInfo vrinfo = SeatManage.Bll.T_SM_ViolateDiscipline.GetViolationRecords(ViolateID);
            if (vrinfo != null)
            {
                vrinfo.Flag = LogStatus.Fail;
                if (!SeatManage.Bll.T_SM_ViolateDiscipline.UpdateViolationRecords(vrinfo))
                {
                    result = Json(new { status = "no", message = "删除失败" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = Json(new { status = "yes", message = "操作成功" }, JsonRequestBehavior.AllowGet);
                }
                //SeatManage.ClassModel.ReaderNoticeInfo rni = new SeatManage.ClassModel.ReaderNoticeInfo();
                //rni.CardNo = vrinfo.CardNo;
                //rni.AddTime = SeatManage.Bll.ServiceDateTime.Now;
                //rni.Note = string.Format("{0}记录的违规，{1}，过期", vrinfo.EnterOutTime, vrinfo.Remark);
                //SeatManage.Bll.T_SM_ReaderNotice.AddReaderNotice(rni);
            }
            else
            {
                result = Json(new { status = "no", message = "操作失败，状态无效" }, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        public JsonResult BespeakLogCancel(string BespeakID)
        {
            JsonResult result = null;
            int id = int.Parse(BespeakID);
            SeatManage.ClassModel.BespeakLogInfo bespeakModel = SeatManage.Bll.T_SM_SeatBespeak.GetBespeaklogById(id);
            if (bespeakModel.BsepeakState != BookingStatus.Waiting)
            {

                result = Json(new { status = "no", message = "操作失败，状态无效" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                bespeakModel.BsepeakState = BookingStatus.Cencaled;
                bespeakModel.CancelPerson = Operation.Admin;
                bespeakModel.CancelTime = SeatManage.Bll.ServiceDateTime.Now;
                bespeakModel.Remark = "被管理员" + this.LoginId + "取消预约";
                if (SeatManage.Bll.T_SM_SeatBespeak.UpdateBespeakList(bespeakModel) > 0)
                {

                    //SeatManage.ClassModel.ReaderNoticeInfo rni = new SeatManage.ClassModel.ReaderNoticeInfo();
                    //rni.CardNo = bespeakModel.CardNo;
                    //rni.AddTime = bespeakModel.CancelTime;
                    //rni.Note = bespeakModel.Remark;
                    //SeatManage.Bll.T_SM_ReaderNotice.AddReaderNotice(rni);

                    //PushMsgInfo msg = new PushMsgInfo();
                    //msg.Title = "您好，您的预约已被取消";
                    //msg.MsgType =  MsgPushType.AdminOperation;
                    //msg.StudentNum = bespeakModel.CardNo;
                    //msg.Message = bespeakModel.Remark;
                    //SeatManage.Bll.T_SM_ReaderNotice.SendPushMsg(msg);
                    result = Json(new { status = "yes", message = "取消成功" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = Json(new { status = "no", message = "操作失败,请重新尝试" }, JsonRequestBehavior.AllowGet);
                }
            }
            return result;
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
            string roomNum = Request.Params["selectRooms"] == null ? "" : Request.Params["selectRooms"]; //ddlReadingRoom.SelectedItem.Value;
            BookingStatus status = Request.Params["selectBespeakState"]==null? BookingStatus.None : (BookingStatus)int.Parse(Request.Params["selectBespeakState"].ToString());
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


        private DataTable GetBlackTable(string starttime, string endtime,bool chkSearchMH,string txtNum,string ddllogstatus)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("CardNo", typeof(string));
            dt.Columns.Add("ReaderName", typeof(string));
            dt.Columns.Add("AddTime", typeof(DateTime));
            dt.Columns.Add("LeaveTime", typeof(DateTime));
            dt.Columns.Add("LeaveMode", typeof(string));
            dt.Columns.Add("LogStatus", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            List<SeatManage.ClassModel.BlackListInfo> Blistlistlist = new List<SeatManage.ClassModel.BlackListInfo>();
            if (chkSearchMH == false)
            {
                Blistlistlist = SeatManage.Bll.T_SM_Blacklist.GetAllBlackListInfo(
                    txtNum,
                    (SeatManage.EnumType.LogStatus)int.Parse(ddllogstatus),
                    starttime,
                    endtime);
            }
            else
            {
                Blistlistlist = SeatManage.Bll.T_SM_Blacklist.GetAllBlackListInfo_ByFuzzySearch(
                    txtNum,
                    (SeatManage.EnumType.LogStatus)int.Parse(ddllogstatus),
                    starttime,
                    endtime);
            }
            foreach (SeatManage.ClassModel.BlackListInfo bllist in Blistlistlist)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = bllist.ID;
                dr["CardNo"] = bllist.CardNo;
                dr["ReaderName"] = bllist.ReaderName;
                dr["AddTime"] = bllist.AddTime;
                dr["LeaveTime"] = bllist.OutTime;
                if (bllist.OutBlacklistMode == SeatManage.EnumType.LeaveBlacklistMode.AutomaticMode)
                {
                    dr["LeaveMode"] = "自动离开";
                }
                else
                {
                    dr["LeaveMode"] = "手动释放";
                }
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

        public string GetBlackData()
        {
            string result = "";
            StringBuilder sb = new StringBuilder();

            string starttime = "";
            if (!string.IsNullOrEmpty(Request.Params["beginDate"]))
            {
                starttime = Request.Params["beginDate"] + " 0:00:00";
            }
            string endtime = "";
            if (!string.IsNullOrEmpty(Request.Params["endDate"]))
            {
                endtime = Request.Params["endDate"] + " 23:59:59";
            }
            if (DateTime.Parse(starttime) >= DateTime.Parse(endtime))
            {
                result = "结束日期必须大于等于开始日期";
            }

            bool chkSearchMH = Request.Params["chkSearchMH"] == null ? false : true;
            string txtNum = Request["txtNum"].Trim();
            string ddllogstatus = Request.Params["ddllogstatus"].Trim();

            DataTable dt = GetBlackTable(starttime, endtime,chkSearchMH,txtNum,ddllogstatus);

            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("{\"ID\": '" + r["ID"] + "',\"CardNo\": '" + r["CardNo"] + "',\"ReaderName\": \"" + r["ReaderName"] + "\",\"AddTime\": \"" + r["AddTime"] + "\",\"LeaveTime\": \"" + r["LeaveTime"] + "\",\"LeaveMode\": \"" + r["LeaveMode"] + "\",\"LogStatus\": \"" + r["LogStatus"] + "\",\"Remark\": \"" + r["Remark"] + "\"}");
                sb.Append(",");
            }
            if (dt.Rows.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            sb.Append("}");
            result = sb.ToString();
            return result;
        }

        public ActionResult Blacklist()
        {
            string nowDay = DateTime.Now.ToShortDateString();
            string before7Day = DateTime.Now.AddDays(-7).ToShortDateString();
            ViewBag.nowDay = nowDay;
            ViewBag.before7Day = before7Day;
            return View();
        }
    }
}