using SeatManage.ClassModel;
using SeatManage.EnumType;
using SeatManageWebQUI.Controllers.FunctionPages.Model;
using System;
using System.Collections.Generic;
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
        public ActionResult SchoolInfo()
        {
            return View();
        }
        public ActionResult LibraryInfo()
        {
            return View();
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

            vm.SeatSelectPos= roomSet.PosTimes.IsUsed.ToString();
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

                    vm.GetType().GetProperty("SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_StartH").SetValue(vm, begintime[0],null);
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

                vm.GetType().GetProperty("ShortLeaveAdMode_Time" + (i + 1) + "_StartH").SetValue(vm, begintime[0],null);
                vm.GetType().GetProperty("ShortLeaveAdMode_Time" + (i + 1) + "_StartM").SetValue(vm, begintime[1], null);
                vm.GetType().GetProperty("ShortLeaveAdMode_Time" + (i + 1) + "_EndH").SetValue(vm, endtime[0], null);
                vm.GetType().GetProperty("ShortLeaveAdMode_Time" + (i + 1) + "_EndM").SetValue(vm, endtime[1], null);
                vm.GetType().GetProperty("ShortLeaveAdMode_Time" + (i + 1)+ "_LeaveTime").SetValue(vm, roomSet.SeatHoldTime.AdvancedSeatHoldTime[i].HoldTimeLength.ToString(), null);
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
            vm.ReadingRoomOpenCloseAdMode= roomSet.RoomOpenSet.UsedAdvancedSet.ToString();
            
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
            vm.SeatTime_Mode = roomSet.SeatUsedTimeLimit.Mode;
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
                vm.SeatBook_SeatBookRadioPercent = "true";
            }
            else if (roomSet.SeatBespeak.BespeakArea.BespeakType == BespeakAreaType.AppointSeat)
            {
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
                vm.AutoLeave = "true";
            }
            else
            {
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
                vm.ReaderLimit_LimitMode_Writelist = "true";
            }
            else
            {
                vm.ReaderLimit_LimitMode_Blacklist = "true";
            }

            //SeatManage.Bll.T_SM_Reader readerbll = new SeatManage.Bll.T_SM_Reader();
            //List<string> readertype = readerbll.GetReaderType();
            //ReaderLimit_ReaderMode.Items.Clear();
            //foreach (string reader in readertype)
            //{
            //    if (string.IsNullOrEmpty(reader))
            //    {
            //        ReaderLimit_ReaderMode.Items.Add("未指定");
            //    }
            //    else
            //    {
            //        ReaderLimit_ReaderMode.Items.Add(reader);
            //    }
            //}
            //string[] readerType = roomSet.LimitReaderEnter.ReaderTypes.Split(';');
            //foreach (ListItem ci in ReaderLimit_ReaderMode.Items)
            //{
            //    foreach (string reader in readerType)
            //    {
            //        if (string.IsNullOrEmpty(reader) && ci.Value == "未指定")
            //        {
            //            ci.Selected = true;
            //            break;
            //        }
            //        else if (ci.Value == reader)
            //        {
            //            ci.Selected = true;
            //            break;
            //        }
            //    }
            //}
            //SameRoomSet.Items.Clear();
            //List<SeatManage.ClassModel.ReadingRoomInfo> rooms = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);
            //foreach (SeatManage.ClassModel.ReadingRoomInfo roominfo in rooms)
            //{
            //    if (roominfo.No != room.No)
            //    {
            //        ListItem li = new ListItem(roominfo.Name + "&nbsp;&nbsp;", roominfo.No);
            //        SameRoomSet.Items.Add(li);
            //    }
            //}
            //for (int i = 0; i < SameRoomSet.Items.Count; i++)
            //{
            //    SameRoomSet.Items[i].Attributes.Add("onmouseover", "showToolTip(event,'" + SameRoomSet.Items[i].Value + "')");
            //}
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
    }
}