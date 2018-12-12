using SeatManage.ClassModel;
using SeatManage.EnumType;
using SeatManageWebQUI.Controllers.FunctionPages.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class SchoolInfoManageController : BaseController
    {
        // GET: SchoolInfoManage
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SaveAddNew()
        {
            JsonResult result = Json(new { status = "yes", message = "保存成功" }, JsonRequestBehavior.AllowGet);

            return result;
        }

        public ActionResult AddNew(string arg)
        {
            ViewBag.Arg = arg;
            return View();
        }

        public string GetSelectSchool()
        {
            List<SeatManage.ClassModel.School> schoollist = SeatManage.Bll.T_SM_School.GetSchoolInfoList(null, null);
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"list\":[");
            foreach (var item in schoollist)
            {
                sb.Append("{\"key\":\"" + item.Name + "\",\"value\":\"" + item.No + "\"},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]}");
            return sb.ToString();
        }

        public string GetSelectLib()
        {
            List<SeatManage.ClassModel.LibraryInfo> listLibrary = new List<SeatManage.ClassModel.LibraryInfo>();
            listLibrary = SeatManage.Bll.T_SM_Library.GetLibraryInfoList(null, null, null);
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"list\":[");
            foreach (var item in listLibrary)
            {
                sb.Append("{\"key\":\"" + item.Name + "\",\"value\":\"" + item.No + "\"},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]}");
            return sb.ToString();
        }

        public string GetTreeData()
        {
            string result = null;
            List<SeatManage.ClassModel.School> schoollist = SeatManage.Bll.T_SM_School.GetSchoolInfoList(null, null);
            List<SeatManage.ClassModel.LibraryInfo> librarylist = SeatManage.Bll.T_SM_Library.GetLibraryInfoList(null, null, null);
            List<SeatManage.ClassModel.ReadingRoomInfo> listReadingRoom = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);
            StringBuilder sb = new StringBuilder();
            sb.Append(" [");
            foreach (var c in schoollist)
            {
                sb.Append("{ id: '" + c.No + "', parentId: '0', name: \"" + c.Name + "\",open: true ,tName:'school'},");
                foreach (var l in librarylist)
                {
                    if (l.School.No == c.No)
                    {
                        sb.Append("{ id: '" + l.No + "', parentId: '" + c.No + "', name: \"" + l.Name + "\",open: true,tName:'lib' },");
                    }
                    foreach (var r in listReadingRoom)
                    {
                        if (r.Libaray.No == l.No)
                        {
                            sb.Append("{ id: '" + r.No + "', parentId: '" + l.No + "', name: \"" + r.Name + "\",open: true ,tName:'room'},");
                        }
                    }
                }
            }
            if (schoollist.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            result = sb.ToString();
            return result;
        }



        #region school manager
        public ActionResult SchoolInfo()
        {
            return View();
        }

        public ActionResult LibraryInfo()
        {
            List<SeatManage.ClassModel.LibraryInfo> librarylist = SeatManage.Bll.T_SM_Library.GetLibraryInfoList(null, null, null);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (LibraryInfo item in librarylist)
            {
                sb.Append("{\"No\": " + item.No + ",\"Name\": \"" + item.Name + "\",\"SchoolName\": \"" + item.School.Name + "\"}");
                sb.Append(",");
            }
            if (librarylist.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            sb.Append("}");
            string data = sb.ToString();
            ViewBag.Data = data;
            return View();
        }

        #endregion
        #region 阅览室设置
        public JsonResult BespeakSeatSettingCanBook(string seatNo, string canBook, string roomNo)
        {
            JsonResult result = null;
            SeatManage.ClassModel.SeatLayout _SeatLayout = SeatManage.Bll.T_SM_SeatBespeak.GetBeseakSeatSettingLayout(roomNo);
            if (canBook == "nobook")
            {

                foreach (SeatManage.ClassModel.Seat seat in _SeatLayout.Seats.Values)
                {
                    if (seat.SeatNo == seatNo)
                    {
                        seat.CanBeBespeak = false;
                        _SeatLayout.RoomNo = roomNo;
                        if (SeatManage.Bll.T_SM_ReadingRoom.UpdateSeatLayout(_SeatLayout) == SeatManage.EnumType.HandleResult.Failed)
                        {
                            result = Json(new { status = "no", message = "设置失败" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            result = Json(new { status = "yes", message = "设置成功" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            else
            {
                foreach (SeatManage.ClassModel.Seat seat in _SeatLayout.Seats.Values)
                {
                    if (seat.SeatNo == seatNo)
                    {
                        seat.CanBeBespeak = true;
                        _SeatLayout.RoomNo = roomNo;
                        if (SeatManage.Bll.T_SM_ReadingRoom.UpdateSeatLayout(_SeatLayout) == SeatManage.EnumType.HandleResult.Failed)
                        {
                            result = Json(new { status = "no", message = "设置失败" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            result = Json(new { status = "yes", message = "设置成功" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            return result;
        }

        public string DrawBespeakSeatSettingLayout(string roomNum, string divTransparentTop, string divTransparentLeft)
        {
            string html = "";
            Code.SeatLayoutTools tool = new Code.SeatLayoutTools();
            html = tool.drowBespeakSeatSettingHtml(roomNum, divTransparentTop, divTransparentLeft);
            return html;
        }

        /// <summary>
        /// 指定（隔天）可预约座位
        /// </summary>
        /// <returns></returns>
        public ViewResult BespeakSeatSetting(string id)
        {
            ViewBag.RoomNo = id;
            return View();
        }

        /// <summary>
        /// 保存阅览室设置
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveReadingRoomSetting()
        {
            JsonResult result = null;

            string currentNo = Request.Params["currentNo"].ToString();
            SeatManage.ClassModel.ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(currentNo);
            if (room == null)
            {
                result = Json(new { status = "no", message = "阅览室设置保存失败" }, JsonRequestBehavior.AllowGet);
            }
            SeatManage.ClassModel.ReadingRoomSetting roomSet = room.Setting;
            if (roomSet == null)
            {
                roomSet = new SeatManage.ClassModel.ReadingRoomSetting();
            }

            string SeatSelectDefaultMode = Request.Params["SeatSelectDefaultMode"].ToString();
            bool SeatSelectAdMode = Request.Params["SeatSelectAdMode"] == null ? false : true;
            bool SeatSelectPos = Request.Params["SeatSelectPos"] == null ? false : true;
            string SelectSeatPosTimes = Request.Params["SelectSeatPosTimes"].ToString();
            string SelectSeatPosCount = Request.Params["SelectSeatPosCount"].ToString();

            roomSet.SeatChooseMethod.DefaultChooseMethod = (SeatManage.EnumType.SelectSeatMode)int.Parse(SeatSelectDefaultMode);
            roomSet.SeatChooseMethod.UsedAdvancedSet = SeatSelectAdMode;
            roomSet.PosTimes.Minutes = int.Parse(SelectSeatPosTimes);
            roomSet.PosTimes.Times = int.Parse(SelectSeatPosCount);
            roomSet.PosTimes.IsUsed = SeatSelectPos;

            //高级设置
            roomSet.SeatChooseMethod.AdvancedSelectSeatMode.Clear();

            for (int dayNum = 0; dayNum < 7; dayNum++)
            {
                SeatChooseMethodPlan scmp = new SeatChooseMethodPlan();
                scmp.Used = Request.Params["SeatSelectAdModeDay" + dayNum] == null ? false : true; //DayCheck.Checked;
                scmp.PlanOption.Clear();
                for (int i = 0; i < 3; i++)
                {
                    string begintimeH = Request.Params["SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_StartH"].ToString();
                    string begintimeM = Request.Params["SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_StartM"].ToString();
                    string endtimeH = Request.Params["SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_EndH"].ToString();
                    string endtimeM = Request.Params["SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_EndM"].ToString();

                    string selectmode = Request.Params["SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_SelectMode"].ToString();
                    if (!string.IsNullOrEmpty(begintimeH) || !string.IsNullOrEmpty(begintimeM) || !string.IsNullOrEmpty(endtimeH) || !string.IsNullOrEmpty(endtimeM))
                    {
                        DateTime begintime = new DateTime();
                        DateTime endtime = new DateTime();
                        if (!DateTime.TryParse(begintimeH + ":" + begintimeM, out begintime))
                        {
                            return Json(new { status = "no", message = "选座设置高级设置，时间设置错误！" }, JsonRequestBehavior.AllowGet);
                        }
                        if (!DateTime.TryParse(endtimeH + ":" + endtimeM, out endtime))
                        {
                            return Json(new { status = "no", message = "选座设置高级设置，时间设置错误！" }, JsonRequestBehavior.AllowGet);
                        }
                        SeatChooseMethodOption scmo = new SeatChooseMethodOption();
                        scmo.ChooseMethod = (SelectSeatMode)int.Parse(selectmode);//(selectmode.SelectedValue);
                        scmo.UsedTime.BeginTime = begintime.ToShortTimeString();
                        scmo.UsedTime.EndTime = endtime.ToShortTimeString();
                        scmp.PlanOption.Add(scmo);
                    }
                }
                roomSet.SeatChooseMethod.AdvancedSelectSeatMode.Add((DayOfWeek)dayNum, scmp);
            }

            //暂离设置
            roomSet.SeatHoldTime.DefaultHoldTimeLength = int.Parse(Request.Params["ShortLeaveDufaultTime"].ToString());
            roomSet.SeatHoldTime.UsedAdvancedSet = Request["ShortLeaveAdMode"] == null ? false : true; //ShortLeaveAdMode.Checked;
            //高级设置
            roomSet.SeatHoldTime.AdvancedSeatHoldTime.Clear();
            for (int i = 0; i < 2; i++)
            {
                string begintimeH = Request.Params["ShortLeaveAdMode_Time" + (i + 1) + "_StartH"];// as TextBox;
                string begintimeM = Request.Params["ShortLeaveAdMode_Time" + (i + 1) + "_StartM"];// as TextBox;
                string endtimeH = Request.Params["ShortLeaveAdMode_Time" + (i + 1) + "_EndH"];//) as TextBox;
                string endtimeM = Request.Params["ShortLeaveAdMode_Time" + (i + 1) + "_EndM"];//) as TextBox;
                string leavetime = Request.Params["ShortLeaveAdMode_Time" + (i + 1) + "_LeaveTime"];//) as TextBox;
                bool used = Request.Params["ShortLeaveAdMode_Time" + (i + 1)] == null ? false : true;//) as CheckBox;

                DateTime begintime = new DateTime();
                DateTime endtime = new DateTime();
                if (!string.IsNullOrEmpty(begintimeH) || !string.IsNullOrEmpty(begintimeM) || !string.IsNullOrEmpty(endtimeH) || !string.IsNullOrEmpty(endtimeM) || !string.IsNullOrEmpty(leavetime))
                {
                    if (!DateTime.TryParse(begintimeH + ":" + begintimeM, out begintime))
                    {
                        return Json(new { status = "no", message = "暂离设置高级设置，时间设置错误！" }, JsonRequestBehavior.AllowGet);

                    }
                    if (!DateTime.TryParse(endtimeH + ":" + endtimeM, out endtime))
                    {
                        return Json(new { status = "no", message = "暂离设置高级设置，时间设置错误！" }, JsonRequestBehavior.AllowGet);
                    }
                    SeatHoldTimeOption shto = new SeatHoldTimeOption();
                    shto.HoldTimeLength = int.Parse(leavetime);
                    shto.Used = used;
                    shto.UsedTime.BeginTime = begintime.ToShortTimeString();
                    shto.UsedTime.EndTime = endtime.ToShortTimeString();
                    roomSet.SeatHoldTime.AdvancedSeatHoldTime.Add(shto);
                }
            }
            roomSet.AdminShortLeave.IsUsed = Request["ShortLeaveByAdmin"] == null ? false : true;
            roomSet.AdminShortLeave.HoldTimeLength = int.Parse(Request.Params["ShortLeaveByAdmin_LeaveTime"].ToString());
            //开闭馆计划设置
            DateTime opentime = new DateTime();
            DateTime closetime = new DateTime();
            if (!DateTime.TryParse(Request.Params["ReadingRoomDufaultOpenTimeH"] + ":" + Request.Params["ReadingRoomDufaultOpenTimeM"], out opentime))
            {
                return Json(new { status = "no", message = "开闭馆计划设置，时间设置错误！" }, JsonRequestBehavior.AllowGet);
            }
            if (!DateTime.TryParse(Request.Params["ReadingRoomDufaultCloseTimeH"] + ":" + Request.Params["ReadingRoomDufaultCloseTimeM"], out closetime))
            {
                return Json(new { status = "no", message = "开闭馆计划设置，时间设置错误！" }, JsonRequestBehavior.AllowGet);
            }
            roomSet.RoomOpenSet.DefaultOpenTime.BeginTime = opentime.ToShortTimeString();
            roomSet.RoomOpenSet.DefaultOpenTime.EndTime = closetime.ToShortTimeString();
            roomSet.RoomOpenSet.OpenBeforeTimeLength = int.Parse(Request.Params["ReadingRoomBeforeOpenTime"]);
            roomSet.RoomOpenSet.CloseBeforeTimeLength = int.Parse(Request.Params["ReadingRoomBeforeCloseTime"]);
            roomSet.RoomOpenSet.UninterruptibleModel = Request.Params["ReadingRoomOpen24H"] == null ? false : true;

            //高级设置
            roomSet.RoomOpenSet.UsedAdvancedSet = Request.Params["ReadingRoomOpenCloseAdMode"] == null ? false : true;
            //foreach (KeyValuePair<DayOfWeek, RoomOpenPlanSet> day in roomSet.RoomOpenSet.RoomOpenPlan)
            //{
            roomSet.RoomOpenSet.RoomOpenPlan.Clear();
            for (int dayNum = 0; dayNum < 7; dayNum++)
            {
                RoomOpenPlanSet rops = new RoomOpenPlanSet();
                bool DayCheck = Request.Params["ReadingRoomAdOpenTime_Day" + dayNum] == null ? false : true;
                rops.Used = DayCheck;
                rops.OpenTime.Clear();
                for (int i = 0; i < 3; i++)
                {
                    string begintimeH = Request.Params["ReadingRoomAdOpenTime_Day" + dayNum + "_Time" + (i + 1) + "_OpenH"];//) as TextBox;
                    string begintimeM = Request.Params["ReadingRoomAdOpenTime_Day" + dayNum + "_Time" + (i + 1) + "_OpenM"];//) as TextBox;
                    string endtimeH = Request.Params["ReadingRoomAdOpenTime_Day" + dayNum + "_Time" + (i + 1) + "_CloseH"];//) as TextBox;
                    string endtimeM = Request.Params["ReadingRoomAdOpenTime_Day" + dayNum + "_Time" + (i + 1) + "_CloseM"];//) as TextBox;
                    bool used = Request.Params["ReadingRoomAdOpenTime_Day" + dayNum + "_Time" + (i + 1)] == null ? false : true;// as CheckBox;
                    if (!string.IsNullOrEmpty(begintimeH) || !string.IsNullOrEmpty(begintimeM) || !string.IsNullOrEmpty(endtimeH) || !string.IsNullOrEmpty(endtimeM))
                    {
                        DateTime begintime = new DateTime();
                        DateTime endtime = new DateTime();
                        if (!DateTime.TryParse(begintimeH + ":" + begintimeM, out begintime))
                        {
                            return Json(new { status = "no", message = "开闭馆计划高级设置，时间设置错误！" }, JsonRequestBehavior.AllowGet);
                        }
                        if (!DateTime.TryParse(endtimeH + ":" + endtimeM, out endtime))
                        {
                            return Json(new { status = "no", message = "开闭馆计划高级设置，时间设置错误！" }, JsonRequestBehavior.AllowGet);
                        }
                        TimeSpace ts = new TimeSpace();
                        ts.BeginTime = begintime.ToShortTimeString();
                        ts.EndTime = endtime.ToShortTimeString();
                        rops.OpenTime.Add(ts);
                    }
                }
                roomSet.RoomOpenSet.RoomOpenPlan.Add((DayOfWeek)dayNum, rops);
            }

            //在座时长设置
            roomSet.SeatUsedTimeLimit.Used = Request.Params["SeatTime"] == null ? false : true;
            roomSet.SeatUsedTimeLimit.Mode = Request.Params["SeatTime_Mode"].ToString();
            roomSet.SeatUsedTimeLimit.UsedTimeLength = int.Parse(Request.Params["SeatTime_SeatTime"]);
            roomSet.SeatUsedTimeLimit.OverTimeHandle = (EnterOutLogType)int.Parse(Request.Params["SeatTime_OverTime_Mode"]);
            roomSet.SeatUsedTimeLimit.IsCanContinuedTime = Request.Params["SeatTime_ContinueTime"] == null ? false : true;
            roomSet.SeatUsedTimeLimit.DelayTimeLength = int.Parse(Request.Params["SeatTime_ContinueTime_Time"]);
            roomSet.SeatUsedTimeLimit.ContinuedTimes = int.Parse(Request.Params["SeatTime_ContinueTime_ContinueCount"]);
            roomSet.SeatUsedTimeLimit.CanDelayTime = int.Parse(Request.Params["SeatTime_ContinueTime_BeforeTime"]);
            roomSet.SeatUsedTimeLimit.FixedTimes.Clear();
            string[] timeSpanList = Request.Params["SeatTime_TimeSpanList"].Split(';');
            for (int i = 0; i < timeSpanList.Length; i++)
            {
                //TextBox timeH = FindControl("PanelSetting").FindControl("SeatTimeSetting").FindControl("SeatTime_TimeH_" + i) as TextBox;
                //TextBox timeM = FindControl("PanelSetting").FindControl("SeatTimeSetting").FindControl("SeatTime_TimeM_" + i) as TextBox;
                if (timeSpanList[i] != "")
                {
                    DateTime dt = new DateTime();
                    if (!DateTime.TryParse(timeSpanList[i], out dt))
                    {
                        return Json(new { status = "no", message = "在座时长设置，时间设置错误！" }, JsonRequestBehavior.AllowGet);
                    }
                    roomSet.SeatUsedTimeLimit.FixedTimes.Add(dt);
                }
            }
            if (roomSet.SeatUsedTimeLimit.Mode != "Free" && roomSet.SeatUsedTimeLimit.Used && roomSet.SeatUsedTimeLimit.FixedTimes.Count == 0)
            {
                return Json(new { status = "no", message = "在座时长设置，时间设置错误！" }, JsonRequestBehavior.AllowGet);
            }

            if (roomSet.RoomOpenSet.UninterruptibleModel)
            {
                if (roomSet.SeatUsedTimeLimit.Mode == "Fixed")
                {
                    return Json(new { status = "no", message = "24小时不间断模式无法兼容在座限时[指定固定时间]模式！请选择[计算在座时长]模式" }, JsonRequestBehavior.AllowGet);
                }
            }

            //预约功能设置
            roomSet.SeatBespeak.Used = Request.Params["SeatBook"] == null ? false : true;
            roomSet.SeatBespeak.AllowDelayTime = Request.Params["ckbDelayTime"] == null ? false : true;
            roomSet.SeatBespeak.AllowLeave = Request.Params["ckbLeave"] == null ? false : true; ;
            roomSet.SeatBespeak.AllowShortLeave = Request.Params["ckbShortLeave"] == null ? false : true;
            roomSet.SeatBespeak.NowDayBespeak = Request.Params["cbNowDayBook"] == null ? false : true;
            roomSet.SeatBespeak.SeatKeepTime = int.Parse(Request.Params["NowDayBookTime"]);
            roomSet.SeatBespeak.BespeakBeforeDays = int.Parse(Request.Params["SeatBook_BeforeBookDay"]);
            DateTime beginbooktime = new DateTime();
            DateTime endbooktime = new DateTime();
            if (!DateTime.TryParse(Request.Params["SeatBook_BookTime_StartH"] + ":" + Request.Params["SeatBook_BookTime_StartM"], out beginbooktime))
            {
                return Json(new { status = "no", message = "预约设置，时间设置错误！" }, JsonRequestBehavior.AllowGet);
            }
            if (!DateTime.TryParse(Request.Params["SeatBook_BookTime_EndH"] + ":" + Request.Params["SeatBook_BookTime_EndM"], out endbooktime))
            {
                return Json(new { status = "no", message = "预约设置，时间设置错误！" }, JsonRequestBehavior.AllowGet);
            }
            roomSet.SeatBespeak.CanBespeatTimeSpace.BeginTime = beginbooktime.ToShortTimeString();
            roomSet.SeatBespeak.CanBespeatTimeSpace.EndTime = endbooktime.ToShortTimeString();
            roomSet.SeatBespeak.ConfirmTime.BeginTime = Request.Params["SeatBook_SubmitBeforeTime"];
            roomSet.SeatBespeak.ConfirmTime.EndTime = Request.Params["SeatBook_SubmitLateTime"];

            roomSet.SeatBespeak.SpecifiedBespeak = Request.Params["cbSpecifiedBook"] == null ? false : true;
            roomSet.SeatBespeak.SelectBespeakSeat = Request.Params["SeatBook_SelectBespeakSeat"] == null ? false : true;
            roomSet.SeatBespeak.SpecifiedTime = Request.Params["SeatBook_SpecifiedTime"] == null ? false : true;
            roomSet.SeatBespeak.BespeatWithOnSeat = Request.Params["SeatBook_BespeakSeatOnSeat"] == null ? false : true;
            roomSet.SeatBespeak.BespeakSeatCount = int.Parse(Request.Params["SeatBook_BespeakSeatCount"]);
            roomSet.SeatBespeak.SpecifiedTimeList.Clear();
            if (Request.Params["SeatBook_SpecifiedTimeList"] != "")
            {
                string[] booktimes = Request.Params["SeatBook_SpecifiedTimeList"].Split(';');
                foreach (string dt in booktimes)
                {
                    DateTime t = new DateTime();
                    if (DateTime.TryParse(dt, out t))
                    {
                        if (roomSet.SeatBespeak.SpecifiedTimeList.Count > 0 && t <= roomSet.SeatBespeak.SpecifiedTimeList[roomSet.SeatBespeak.SpecifiedTimeList.Count - 1])
                        {
                            return Json(new { status = "no", message = "预约设置，指定时段设置错误！" }, JsonRequestBehavior.AllowGet);
                        }
                        roomSet.SeatBespeak.SpecifiedTimeList.Add(t);
                    }
                }
            }
            if (roomSet.SeatBespeak.SpecifiedTimeList.Count < 1 && roomSet.SeatBespeak.SpecifiedTime)
            {
                return Json(new { status = "no", message = "预约设置，请设置指定的预约时间！" }, JsonRequestBehavior.AllowGet);
            }

            string isSeatBook_SeatBookRadioPercent = Request.Params["SeatBespeak"];// == null ? false : true;
            if (isSeatBook_SeatBookRadioPercent == "Percentage")
            {
                roomSet.SeatBespeak.BespeakArea.BespeakType = BespeakAreaType.Percentage;
            }
            else //if (Request.Params["SeatBook_SeatBookRadioSetted"]==null?false:true)
            {
                roomSet.SeatBespeak.BespeakArea.BespeakType = BespeakAreaType.AppointSeat;
            }
            roomSet.SeatBespeak.BespeakArea.Scale = double.Parse(Request.Params["SeatBook_SeatBookRadioPercent_Percent"]) / 100;
            roomSet.SeatBespeak.NoBespeakDates.Clear();
            if (!string.IsNullOrEmpty(Request.Params["SeatBook_CanNotSeatBookDate"]))
            {
                string[] cannotbookdate = Request.Params["SeatBook_CanNotSeatBookDate"].Split(';');
                foreach (string date in cannotbookdate)
                {
                    string[] datespan = date.Split('~');
                    DateTime begindate = new DateTime();
                    DateTime enddate = new DateTime();
                    if (datespan.Length > 1)
                    {
                        if (!DateTime.TryParse(datespan[0], out begindate))
                        {
                            return Json(new { status = "no", message = "预约设置，不可预约时间设置错误！" }, JsonRequestBehavior.AllowGet);
                        }
                        if (!DateTime.TryParse(datespan[1], out enddate))
                        {
                            return Json(new { status = "no", message = "预约设置，不可预约时间设置错误！" }, JsonRequestBehavior.AllowGet);
                        }
                        TimeSpace ts = new TimeSpace();
                        ts.BeginTime = begindate.Month.ToString() + "-" + begindate.Day.ToString();
                        ts.EndTime = enddate.Month.ToString() + "-" + enddate.Day.ToString();
                        roomSet.SeatBespeak.NoBespeakDates.Add(ts);
                    }
                    else
                    {
                        return Json(new { status = "no", message = "预约设置，不可预约时间设置错误！" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            //黑名单设置
            roomSet.UsedBlacklistLimit = Request.Params["UseBlacklist"] == null ? false : true;
            roomSet.IsRecordViolate = Request.Params["IsRecordViolate"] == null ? false : true;
            roomSet.BlackListSetting.Used = Request.Params["UseBlacklistSetting"] == null ? false : true;
            if (roomSet.BlackListSetting.Used)
            {
                roomSet.UsedBlacklistLimit = true;
                roomSet.IsRecordViolate = true;
            }
            roomSet.BlackListSetting.ViolateTimes = int.Parse(Request.Params["RecordViolateCount"]);
            roomSet.BlackListSetting.LimitDays = int.Parse(Request.Params["LeaveBlackDays"]);
            roomSet.BlackListSetting.ViolateFailDays = int.Parse(Request.Params["LeaveRecordViolateDays"]);
            bool isAutoLeave = Request.Params["AutoLeave"] == null ? false : true;
            if (isAutoLeave)
            {
                roomSet.BlackListSetting.LeaveBlacklist = LeaveBlacklistMode.AutomaticMode;
            }
            else
            {
                roomSet.BlackListSetting.LeaveBlacklist = LeaveBlacklistMode.ManuallyMode;
            }
            roomSet.BlackListSetting.ViolateRoule[ViolationRecordsType.BookingTimeOut] = Request.Params["RecordViolate_BookOverTime"] == null ? false : true;
            roomSet.BlackListSetting.ViolateRoule[ViolationRecordsType.LeaveByAdmin] = Request.Params["RecordViolate_LeaveByAdmin"] == null ? false : true;// RecordViolate_LeaveByAdmin.Checked;
            roomSet.BlackListSetting.ViolateRoule[ViolationRecordsType.SeatOutTime] = Request.Params["RecordViolate_SeatOverTime"] == null ? false : true;// RecordViolate_SeatOverTime.Checked;
            roomSet.BlackListSetting.ViolateRoule[ViolationRecordsType.ShortLeaveByAdminOutTime] = Request.Params["RecordViolate_ShortLeaveByAdmin"] == null ? false : true; //RecordViolate_ShortLeaveByAdmin.Checked;
            roomSet.BlackListSetting.ViolateRoule[ViolationRecordsType.ShortLeaveByReaderOutTime] = Request.Params["RecordViolate_ShortLeaveByReader"] == null ? false : true; //RecordViolate_ShortLeaveByReader.Checked;
            roomSet.BlackListSetting.ViolateRoule[ViolationRecordsType.ShortLeaveOutTime] = Request.Params["RecordViolate_ShortLeaveOverTime"] == null ? false : true; //RecordViolate_ShortLeaveOverTime.Checked;

            //其他设置
            roomSet.SeatNumAmount = int.Parse(Request.Params["ShowSeatNumberCount"]);
            roomSet.NoManagement.Used = Request.Params["NoManMode"] == null ? false : true;
            roomSet.NoManagement.OperatingInterval = double.Parse(Request.Params["NoManMode_WaitTime"]);
            roomSet.LimitReaderEnter.Used = Request.Params["ReaderLimit"] == null ? false : true;

            bool isReaderLimit_LimitMode_Writelist = Request.Params["ReaderLimit_LimitMode_Writelist"] == null ? false : true;
            if (isReaderLimit_LimitMode_Writelist)
            {
                roomSet.LimitReaderEnter.CanEnter = true;
            }
            else
            {
                roomSet.LimitReaderEnter.CanEnter = false;
            }
            roomSet.LimitReaderEnter.ReaderTypes = "";
            //foreach (ListItem type in ReaderLimit_ReaderMode.Items)
            //{
            //    if (type.Selected)
            //    {
            //        if (!string.IsNullOrEmpty(roomSet.LimitReaderEnter.ReaderTypes))
            //        {
            //            roomSet.LimitReaderEnter.ReaderTypes += ";";
            //        }
            //        roomSet.LimitReaderEnter.ReaderTypes += type.Value;
            //    }
            //}
            roomSet.LimitReaderEnter.ReaderTypes = roomSet.LimitReaderEnter.ReaderTypes.Replace("未指定", "");
            room.Setting = roomSet;

            //SameRoomSet_101004
            if (SeatManage.Bll.T_SM_ReadingRoom.UpdateReadingRoom(room))
            {
                result = Json(new { status = "yes", message = "阅览室[" + room.Name + "]设置成功" }, JsonRequestBehavior.AllowGet);
                List<SeatManage.ClassModel.ReadingRoomInfo> rooms = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);
                foreach (ReadingRoomInfo item in rooms)
                {
                    bool isSelected = Request.Params["SameRoomSet_" + item.No] == null ? false : true;
                    if (isSelected)
                    {
                        item.Setting = roomSet;
                        if (SeatManage.Bll.T_SM_ReadingRoom.UpdateReadingRoom(item))
                        {

                            result = Json(new { status = "yes", message = "阅览室批量操作成功" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            result = Json(new { status = "no", message = "保存失败" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            else
            {
                result = Json(new { status = "no", message = "阅览室[" + room.Name + "]保存失败" }, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        /// <summary>
        /// 阅览室设置界面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult ReadingRoomSetting(string id)
        {
            AuthorizeVerify.FunctionAuthorizeInfo authorize = SeatManage.Bll.AuthorizationOperation.GetFunctionAuthorize();
            SeatManage.ClassModel.ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(id);
            if (room == null)
            {
                room = new SeatManage.ClassModel.ReadingRoomInfo();
            }
            SeatManage.ClassModel.ReadingRoomSetting roomSet = room.Setting;
            if (roomSet == null)
            {
                roomSet = new SeatManage.ClassModel.ReadingRoomSetting();
            }
            RoomSettingViewModel vm = new RoomSettingViewModel();

            //选座模式设置
            vm.SeatSelectDefaultMode = ((int)roomSet.SeatChooseMethod.DefaultChooseMethod).ToString();

            vm.SeatSelectPos = roomSet.PosTimes.IsUsed.ToString();
            vm.SelectSeatPosTimes = roomSet.PosTimes.Minutes.ToString();
            vm.SelectSeatPosCount = roomSet.PosTimes.Times.ToString();

            vm.SeatSelectAdMode = roomSet.SeatChooseMethod.UsedAdvancedSet.ToString();
            //高级设置
            foreach (KeyValuePair<DayOfWeek, SeatChooseMethodPlan> day in roomSet.SeatChooseMethod.AdvancedSelectSeatMode)
            {
                string dayNum = ((int)day.Value.Day).ToString();
                vm.GetType().GetProperty("SeatSelectAdModeDay" + dayNum).SetValue(vm, day.Value.Used.ToString(), null);

                for (int i = 0; i < day.Value.PlanOption.Count; i++)
                {
                    string[] begintime = day.Value.PlanOption[i].UsedTime.BeginTime.Split(':');
                    string[] endtime = day.Value.PlanOption[i].UsedTime.EndTime.Split(':');

                    vm.GetType().GetProperty("SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_StartH").SetValue(vm, begintime[0], null);
                    vm.GetType().GetProperty("SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_StartM").SetValue(vm, begintime[1], null);
                    vm.GetType().GetProperty("SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_EndH").SetValue(vm, endtime[0], null);
                    vm.GetType().GetProperty("SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_EndM").SetValue(vm, endtime[1], null);
                    vm.GetType().GetProperty("SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_SelectMode").SetValue(vm, ((int)day.Value.PlanOption[i].ChooseMethod).ToString(), null);
                }
            }
            //暂离设置
            vm.ShortLeaveDufaultTime = roomSet.SeatHoldTime.DefaultHoldTimeLength.ToString();
            vm.ShortLeaveAdMode = roomSet.SeatHoldTime.UsedAdvancedSet.ToString();

            //高级设置
            for (int i = 0; i < roomSet.SeatHoldTime.AdvancedSeatHoldTime.Count; i++)
            {
                string[] begintime = roomSet.SeatHoldTime.AdvancedSeatHoldTime[i].UsedTime.BeginTime.Split(':');
                string[] endtime = roomSet.SeatHoldTime.AdvancedSeatHoldTime[i].UsedTime.EndTime.Split(':');

                vm.GetType().GetProperty("ShortLeaveAdMode_Time" + (i + 1) + "_StartH").SetValue(vm, begintime[0], null);
                vm.GetType().GetProperty("ShortLeaveAdMode_Time" + (i + 1) + "_StartM").SetValue(vm, begintime[1], null);
                vm.GetType().GetProperty("ShortLeaveAdMode_Time" + (i + 1) + "_EndH").SetValue(vm, endtime[0], null);
                vm.GetType().GetProperty("ShortLeaveAdMode_Time" + (i + 1) + "_EndM").SetValue(vm, endtime[1], null);
                vm.GetType().GetProperty("ShortLeaveAdMode_Time" + (i + 1) + "_LeaveTime").SetValue(vm, roomSet.SeatHoldTime.AdvancedSeatHoldTime[i].HoldTimeLength.ToString(), null);
                vm.GetType().GetProperty("ShortLeaveAdMode_Time" + (i + 1)).SetValue(vm, roomSet.SeatHoldTime.AdvancedSeatHoldTime[i].Used.ToString(), null);
            }
            //开闭馆计划设置
            string[] opentime = roomSet.RoomOpenSet.DefaultOpenTime.BeginTime.Split(':');
            string[] closetime = roomSet.RoomOpenSet.DefaultOpenTime.EndTime.Split(':');

            vm.ReadingRoomDufaultOpenTimeH = opentime[0];
            vm.ReadingRoomDufaultOpenTimeM = opentime[1];
            vm.ReadingRoomBeforeOpenTime = roomSet.RoomOpenSet.OpenBeforeTimeLength.ToString();
            vm.ReadingRoomDufaultCloseTimeH = closetime[0];
            vm.ReadingRoomDufaultCloseTimeM = closetime[1];
            vm.ReadingRoomBeforeCloseTime = roomSet.RoomOpenSet.CloseBeforeTimeLength.ToString();
            vm.ReadingRoomOpen24H = roomSet.RoomOpenSet.UninterruptibleModel.ToString();
            //验证授权
            //if (authorize != null && !authorize.SystemFunction.Contains("RoomOC_24HModel"))
            //{
            //    open24htr.Style["display"] = "none";
            //}
            //高级设置
            vm.ReadingRoomOpenCloseAdMode = roomSet.RoomOpenSet.UsedAdvancedSet.ToString();

            foreach (KeyValuePair<DayOfWeek, RoomOpenPlanSet> day in roomSet.RoomOpenSet.RoomOpenPlan)
            {
                string dayNum = ((int)day.Value.Day).ToString();
                vm.GetType().GetProperty("ReadingRoomAdOpenTime_Day" + dayNum).SetValue(vm, day.Value.Used.ToString(), null);
                for (int i = 0; i < day.Value.OpenTime.Count; i++)
                {
                    string[] begintime = day.Value.OpenTime[i].BeginTime.Split(':');
                    string[] endtime = day.Value.OpenTime[i].EndTime.Split(':');
                    vm.GetType().GetProperty("ReadingRoomAdOpenTime_Day" + dayNum + "_Time" + (i + 1) + "_OpenH").SetValue(vm, begintime[0], null);
                    vm.GetType().GetProperty("ReadingRoomAdOpenTime_Day" + dayNum + "_Time" + (i + 1) + "_OpenM").SetValue(vm, begintime[1], null);
                    vm.GetType().GetProperty("ReadingRoomAdOpenTime_Day" + dayNum + "_Time" + (i + 1) + "_CloseH").SetValue(vm, endtime[0], null);
                    vm.GetType().GetProperty("ReadingRoomAdOpenTime_Day" + dayNum + "_Time" + (i + 1) + "_CloseM").SetValue(vm, endtime[1], null);
                }
            }
            vm.ShortLeaveByAdmin = roomSet.AdminShortLeave.IsUsed.ToString();
            vm.ShortLeaveByAdmin_LeaveTime = roomSet.AdminShortLeave.HoldTimeLength.ToString();
            //在座时长设置
            vm.SeatTime = roomSet.SeatUsedTimeLimit.Used.ToString();
            vm.SeatTime_Mode = roomSet.SeatUsedTimeLimit.Mode; //Free
            vm.SeatTime_SeatTime = roomSet.SeatUsedTimeLimit.UsedTimeLength.ToString();
            vm.SeatTime_OverTime_Mode = ((int)roomSet.SeatUsedTimeLimit.OverTimeHandle).ToString();
            vm.SeatTime_ContinueTime = roomSet.SeatUsedTimeLimit.IsCanContinuedTime.ToString();
            vm.SeatTime_ContinueTime_Time = roomSet.SeatUsedTimeLimit.DelayTimeLength.ToString();
            vm.SeatTime_ContinueTime_ContinueCount = roomSet.SeatUsedTimeLimit.ContinuedTimes.ToString();
            vm.SeatTime_ContinueTime_BeforeTime = roomSet.SeatUsedTimeLimit.CanDelayTime.ToString();
            for (int i = 0; i < roomSet.SeatUsedTimeLimit.FixedTimes.Count; i++)
            {
                vm.SeatTime_TimeSpanList += roomSet.SeatUsedTimeLimit.FixedTimes[i].ToShortTimeString() + ";";
            }
            //预约功能设置ckbDelayTime
            vm.ckbDelayTime = roomSet.SeatBespeak.AllowDelayTime.ToString();
            vm.ckbLeave = roomSet.SeatBespeak.AllowLeave.ToString();
            vm.ckbShortLeave = roomSet.SeatBespeak.AllowShortLeave.ToString();

            vm.SeatBook = roomSet.SeatBespeak.Used.ToString();

            vm.SeatBook_BeforeBookDay = roomSet.SeatBespeak.BespeakBeforeDays.ToString();
            string[] beginbooktime = roomSet.SeatBespeak.CanBespeatTimeSpace.BeginTime.Split(':');
            string[] endbooktime = roomSet.SeatBespeak.CanBespeatTimeSpace.EndTime.Split(':');

            vm.SeatBook_BookTime_StartH = beginbooktime[0];
            vm.SeatBook_BookTime_StartM = beginbooktime[1];
            vm.SeatBook_BookTime_EndH = endbooktime[0];
            vm.SeatBook_BookTime_EndM = endbooktime[1];
            vm.SeatBook_SubmitBeforeTime = roomSet.SeatBespeak.ConfirmTime.BeginTime;
            vm.SeatBook_SubmitLateTime = roomSet.SeatBespeak.ConfirmTime.EndTime;

            if (roomSet.SeatBespeak.BespeakArea.BespeakType == BespeakAreaType.Percentage)
            {
                vm.SeatBespeak = "Percentage";

                vm.SeatBook_SeatBookRadioPercent = "true";
            }
            else if (roomSet.SeatBespeak.BespeakArea.BespeakType == BespeakAreaType.AppointSeat)
            {
                vm.SeatBespeak = "AppointSeat";
                vm.SeatBook_SeatBookRadioSetted = "true";
            }
            vm.SeatBook_SeatBookRadioPercent_Percent = ((roomSet.SeatBespeak.BespeakArea.Scale) * 100).ToString();

            foreach (SeatManage.ClassModel.TimeSpace cannotbookdate in roomSet.SeatBespeak.NoBespeakDates)
            {
                if (!string.IsNullOrEmpty(vm.SeatBook_CanNotSeatBookDate))
                {
                    vm.SeatBook_CanNotSeatBookDate += ";";
                }
                vm.SeatBook_CanNotSeatBookDate += cannotbookdate.BeginTime + "~" + cannotbookdate.EndTime;
            }
            vm.cbNowDayBook = roomSet.SeatBespeak.NowDayBespeak.ToString();
            vm.NowDayBookTime = roomSet.SeatBespeak.SeatKeepTime.ToString().ToString();
            vm.cbSpecifiedBook = roomSet.SeatBespeak.SpecifiedBespeak.ToString();
            vm.SeatBook_SelectBespeakSeat = roomSet.SeatBespeak.SelectBespeakSeat.ToString();
            vm.SeatBook_SpecifiedTime = roomSet.SeatBespeak.SpecifiedTime.ToString();
            vm.SeatBook_BespeakSeatOnSeat = roomSet.SeatBespeak.BespeatWithOnSeat.ToString();
            vm.SeatBook_BespeakSeatCount = roomSet.SeatBespeak.BespeakSeatCount.ToString();
            foreach (DateTime dt in roomSet.SeatBespeak.SpecifiedTimeList)
            {
                if (vm.SeatBook_SpecifiedTimeList != "")
                {
                    vm.SeatBook_SpecifiedTimeList += ";";
                }
                vm.SeatBook_SpecifiedTimeList += dt.ToShortTimeString();
            }


            //黑名单设置
            vm.UseBlacklist = roomSet.UsedBlacklistLimit.ToString();
            vm.IsRecordViolate = roomSet.IsRecordViolate.ToString();
            vm.UseBlacklistSetting = roomSet.BlackListSetting.Used.ToString();

            vm.RecordViolateCount = roomSet.BlackListSetting.ViolateTimes.ToString();
            vm.LeaveBlackDays = roomSet.BlackListSetting.LimitDays.ToString();
            vm.LeaveRecordViolateDays = roomSet.BlackListSetting.ViolateFailDays.ToString();
            if (roomSet.BlackListSetting.LeaveBlacklist == LeaveBlacklistMode.AutomaticMode)
            {
                vm.leaveblacklist = "AutoLeave";
                vm.AutoLeave = "true";
            }
            else
            {
                vm.leaveblacklist = "HardLeave";
                vm.HardLeave = "true";
            }
            vm.RecordViolate_BookOverTime = roomSet.BlackListSetting.ViolateRoule[ViolationRecordsType.BookingTimeOut].ToString();
            vm.RecordViolate_LeaveByAdmin = roomSet.BlackListSetting.ViolateRoule[ViolationRecordsType.LeaveByAdmin].ToString();
            vm.RecordViolate_SeatOverTime = roomSet.BlackListSetting.ViolateRoule[ViolationRecordsType.SeatOutTime].ToString();
            vm.RecordViolate_ShortLeaveByAdmin = roomSet.BlackListSetting.ViolateRoule[ViolationRecordsType.ShortLeaveByAdminOutTime].ToString();
            vm.RecordViolate_ShortLeaveByReader = roomSet.BlackListSetting.ViolateRoule[ViolationRecordsType.ShortLeaveByReaderOutTime].ToString();
            vm.RecordViolate_ShortLeaveOverTime = roomSet.BlackListSetting.ViolateRoule[ViolationRecordsType.ShortLeaveOutTime].ToString();
            //其他设置
            vm.ShowSeatNumberCount = roomSet.SeatNumAmount.ToString();
            vm.NoManMode = roomSet.NoManagement.Used.ToString();
            vm.NoManMode_WaitTime = roomSet.NoManagement.OperatingInterval.ToString();

            vm.ReaderLimit = roomSet.LimitReaderEnter.Used.ToString();
            if (roomSet.LimitReaderEnter.CanEnter)
            {
                vm.limitReader = "ReaderLimit_LimitMode_Writelist";
                vm.ReaderLimit_LimitMode_Writelist = "true";
            }
            else
            {
                vm.limitReader = "ReaderLimit_LimitMode_Blacklist";
                vm.ReaderLimit_LimitMode_Blacklist = "true";
            }

            List<SeatManage.ClassModel.ReadingRoomInfo> rooms = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);

            System.Text.StringBuilder sbHtml = new StringBuilder();
            foreach (SeatManage.ClassModel.ReadingRoomInfo roominfo in rooms)
            {
                if (roominfo.No != room.No)
                {
                    sbHtml.Append("<input type=\"checkbox\"  ID=\"SameRoomSet_" + roominfo.No + "\" name=\"SameRoomSet_" + roominfo.No + "\" /><label for=\"SameRoomSet_" + roominfo.No + "\" class=\"hand\">" + roominfo.Name + "</label>");
                }
            }
            ViewBag.RoomListHtml = sbHtml.ToString();
            ViewBag.RoomNo = id;
            ViewBag.VM = vm;
            return View();
        }

        /// <summary>
        /// 绑定阅览室列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ReadingRoomInfo()
        {
            List<SeatManage.ClassModel.ReadingRoomInfo> listReadingRoom = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (var item in listReadingRoom)
            {
                sb.Append("{\"No\": " + item.No + ",\"Name\": \"" + item.Name + "\",\"Libaray\": \"" + item.Libaray.Name + "\",\"School\": \"" + item.Libaray.School.Name + "\"}");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append("}");
            string data = sb.ToString();
            ViewBag.Data = data;

            return View();
        } 
        #endregion
    }
}