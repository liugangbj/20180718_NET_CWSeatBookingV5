using SeatManage.ClassModel;
using SeatManage.EnumType;
using SeatManageWebQUI.Controllers.FunctionPages.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class RegulationRulesSettingController : BaseController
    {
        // GET: RegulationRulesSetting
        public ActionResult Index()
        {
            return View();
        }
       private List<SeatManage.ClassModel.TerminalInfoV2> clientlist = SeatManage.Bll.TerminalOperatorService.GetAllTeminalInfo();

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="term"></param>
        private TerminalInfoV2 NewSetting(TerminalInfoV2 term)
        {
            TerminalInfoV2 newterm = term;
            newterm.DeviceSetting.IsShowInitPOS = Request.Params["cbipos"] == null ? false : true;//cbipos.Checked;
            newterm.DeviceSetting.UsingPrintSlip = Request.Params["rbprint"] == null ? PrintSlipMode.None : (PrintSlipMode)int.Parse(Request.Params["rbprint"]);// SeatManage.EnumType.EnterOutLogType.ShortLeave; //(SeatManage.EnumType.EnterOutLogType)int.Parse(ddlleavemode.SelectedValue);//(SeatManage.EnumType.PrintSlipMode)int.Parse(rbprint.SelectedValue);
            newterm.DeviceSetting.UsingEnterNoForSeat = Request.Params["cbNumSelect"] == null ? false : true;// cbNumSelect.Checked;
            newterm.DeviceSetting.SelectMethod = Request.Params["rblSelectSeatMode"] == null ? SelectSeatMode.ManualMode : (SelectSeatMode)int.Parse(Request.Params["rblSelectSeatMode"]);//(SeatManage.EnumType.SelectSeatMode)int.Parse(rblSelectSeatMode.SelectedValue);
            newterm.DeviceSetting.UsingActiveBespeakSeat = Request.Params["cbBespeak"] == null ? false : true;//cbBespeak.Checked;
            newterm.DeviceSetting.UsingOftenUsedSeat.Used = Request.Params["cbOftenSeat"] == null ? false : true;// cbOftenSeat.Checked;
            newterm.DeviceSetting.UsingOftenUsedSeat.LengthDays = Request.Params["nbostime"] ==null?15:int.Parse(Request.Params["nbostime"]);
            newterm.DeviceSetting.UsingOftenUsedSeat.SeatCount = Request.Params["nboscont"] == null ? 12 : int.Parse(Request.Params["nboscont"]);
            newterm.DeviceSetting.PosTimes.Minutes = Request.Params["numSelectSeatTime"] == null ? 10 : int.Parse(Request.Params["numSelectSeatTime"]);// int.Parse(numSelectSeatTime.Text);
            newterm.DeviceSetting.PosTimes.Times = Request.Params["numSelectSeatCont"] == null ? 3 : int.Parse(Request.Params["numSelectSeatCont"]); //int.Parse(numSelectSeatCont.Text);
            newterm.DeviceSetting.PosTimes.IsUsed = Request.Params["cbSelectSeatCount"] == null ? false : true;// cbSelectSeatCount.Checked;

            //if (rblfbl.SelectedValue == "0")
            //{
            //    newterm.DeviceSetting.SystemResoultion = new ResolutionV2();
            //    string[] xy = txtReDiy.Text.Split('x');
            //    if (xy.Length > 1)
            //    {
            //        int w = 0;
            //        int h = 0;
            //        if (int.TryParse(xy[0], out w) && int.TryParse(xy[1], out h))
            //        {
            //            newterm.DeviceSetting.SystemResoultion.WindowSize.Size.X = w;
            //            newterm.DeviceSetting.SystemResoultion.WindowSize.Size.Y = h;
            //            newterm.DeviceSetting.SystemResoultion.WindowSize.Location.X = 0;
            //            newterm.DeviceSetting.SystemResoultion.WindowSize.Location.Y = 0;
            //            newterm.DeviceSetting.SystemResoultion.TooltipSize.Location.X = 0;
            //            newterm.DeviceSetting.SystemResoultion.TooltipSize.Location.Y = 0;
            //            newterm.DeviceSetting.SystemResoultion.TooltipSize.Size.X = 0;
            //            newterm.DeviceSetting.SystemResoultion.TooltipSize.Size.Y = 0;
            //        }
            //        else
            //        {
            //            return null;
            //        }
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
            //else
            //{
            //    newterm.DeviceSetting.SystemResoultion = new ResolutionV2(rblfbl.SelectedValue);
            //}
            //newterm.DeviceSetting.Rooms.Clear();
            //foreach (FineUI.CheckItem item in clbroom.Items)
            //{
            //    if (item.Selected)
            //    {
            //        newterm.DeviceSetting.Rooms.Add(item.Value);
            //    }
            //}
            return newterm;
        }

        public JsonResult SaveDeviceSetting()
        {
            JsonResult result = null;



            return result;
        }


        public JsonResult GetDeviceInfoData(string ClientNo)
        {
            JsonResult result = null;
            string cNo = ClientNo;
            if (cNo == "0")
            {
                cNo = clientlist[0].ClientNo;
            }

            TerminalInfoV2 term = SeatManage.Bll.TerminalOperatorService.GetTeminalSetting(cNo);
            DeviceInfoModel showModel = new DeviceInfoModel();
            showModel.ClientNo = term.ClientNo;//终端机编号
            showModel.SelesctALLtem = "false"; //默认先设定最下方关联机器不选
            showModel.Cbselectallrr = "false";//默认先设定最下方关联阅览室不选
            showModel.TxtRemark = term.Describe; //机器备注
            showModel.SelectMethod = ((int)term.DeviceSetting.SelectMethod).ToString(); //选座方式
            showModel.CbSelectSeatCountChecked = term.DeviceSetting.PosTimes.IsUsed ? "Y" : "N"; //选座次数限定是否启用
            showModel.NumSelectSeatTimeText = term.DeviceSetting.PosTimes.Minutes.ToString();//选座次数限定启用的时间分钟数
            showModel.NumSelectSeatContText = term.DeviceSetting.PosTimes.Times.ToString();//选座次数限定启用的选座次数
            showModel.CbOftenSeatChecked = term.DeviceSetting.UsingOftenUsedSeat.Used ? "Y" : "N"; //是否启用常用座位
            showModel.NbostimeText = term.DeviceSetting.UsingOftenUsedSeat.LengthDays.ToString(); //常用座位记录天数
            showModel.NboscontText = term.DeviceSetting.UsingOftenUsedSeat.SeatCount.ToString(); //常用座位记录人次
            showModel.RbprintSelectedValue = ((int)term.DeviceSetting.UsingPrintSlip).ToString();
            showModel.CbBespeakChecked = term.DeviceSetting.UsingActiveBespeakSeat ? "Y" : "N";
            showModel.CbiposChecked = term.DeviceSetting.IsShowInitPOS ? "Y" : "N";
            showModel.CbNumSelectChecked = term.DeviceSetting.UsingEnterNoForSeat ? "Y" : "N";
            bool isSelect = false;
            switch (term.DeviceSetting.SystemResoultion.WindowSize.Size.X)
            {
                case 1080:
                    isSelect = true;
                    break;
                case 1024:
                    isSelect = true;
                    break;
                case 1280:
                    isSelect = true;
                    break;
                case 1440:
                    isSelect = true;
                    break;
                case 1920:
                    isSelect = true;
                    break;
            }
            showModel.RblfblSelectedValue = isSelect ? term.DeviceSetting.SystemResoultion.WindowSize.Size.X.ToString() : "0";
            showModel.TxtReDiy = "";
            if (!isSelect) showModel.TxtReDiy = term.DeviceSetting.SystemResoultion.WindowSize.Size.X + "x" + term.DeviceSetting.SystemResoultion.WindowSize.Size.Y;
            List<SeatManage.ClassModel.ReadingRoomInfo> roomlist = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);
            StringBuilder sbHtmlRooms = new StringBuilder();
            int roomCount = 1;
            foreach (var roominfo in roomlist)
            {
                if (term.DeviceSetting.Rooms.Contains(roominfo.No))//包含
                {
                    sbHtmlRooms.Append("<input type=\"checkbox\" checked=\"checked\"  ID=\"SameRoomSet_" + roominfo.No + "\" name=\"SameRoomSet_" + roominfo.No + "\" /><label for=\"SameRoomSet_" + roominfo.No + "\" class=\"hand\">" + roominfo.Name + "[" + roominfo.No + "]</label>");
                }
                else
                {
                    sbHtmlRooms.Append("<input type=\"checkbox\"  ID=\"SameRoomSet_" + roominfo.No + "\" name=\"SameRoomSet_" + roominfo.No + "\" /><label for=\"SameRoomSet_" + roominfo.No + "\" class=\"hand\">" + roominfo.Name + "[" + roominfo.No + "]</label>");
                }
                if (roomCount % 3 == 0) sbHtmlRooms.Append("<br/>");
                roomCount++;
            }
            showModel.HtmlRooms = sbHtmlRooms.ToString();

            StringBuilder sbHtmlDevice = new StringBuilder();
            foreach (var item in clientlist)
            {
                if (item.ClientNo != ClientNo)
                {
                    sbHtmlDevice.Append("<input type=\"checkbox\"  ID=\"SameDeviceSet_" + item.ClientNo + "\" name=\"SameDeviceSet_" + item.ClientNo + "\" /><label for=\"SameDeviceSet_" + item.ClientNo + "\" class=\"hand\">" + item.Describe + "[" + item.ClientNo + "]</label><br/>");
                }
            }
            showModel.HtmlDevice = sbHtmlDevice.ToString();
            result = Json(new { status = "yes", message = "查询成功", showModel }, JsonRequestBehavior.AllowGet);
            return result;
        }

        public string GetDeviceInfoTreeData()
        {
            string result = null;
            StringBuilder sb = new StringBuilder();
        //    List<SeatManage.ClassModel.TerminalInfoV2> clientlist = SeatManage.Bll.TerminalOperatorService.GetAllTeminalInfo();
            sb.Append(" [");
            sb.Append(" { id:1,  parentId:0,open: true , name:\"选座机列表\"},");
            foreach (var item in clientlist)
            {
                string desc = string.IsNullOrEmpty(item.Describe) ? item.ClientNo : item.Describe;
                sb.Append(" { id:"+item.ClientNo+",  parentId:1, name:\""+ desc + "\"},");
            }
            if (clientlist.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            result = sb.ToString();
            return result;
        }



        public ActionResult DeviceInfo()
        {
            return View();
        }

        public JsonResult SaveAccessSetting()
        {
            JsonResult result = null;

            SeatManage.ClassModel.AccessSetting accset = new SeatManage.ClassModel.AccessSetting();
            accset.IsUsed = Request.Params["IsASUserd"] == null ? false : true;// Request.Params["IsASUserd"];// IsASUserd.Checked;
            accset.EnterLib = Request.Params["IsELUserd"] == null ? false : true;// IsELUserd.Checked;
            accset.OutLib = Request.Params["IsOLUserd"] == null ? false : true;// IsOLUserd.Checked;
            accset.IsLimitBlackList = Request.Params["cbBLIsUsed"] == null ? false : true;// cbBLIsUsed.Checked;
            accset.AddViolationRecords = Request.Params["IsAddrv"] == null ? false : true;// IsAddrv.Checked;
            accset.LeaveTimeSpan = Request.Params["LeaveTimeForm3"] == null ? 5 : int.Parse(Request.Params["LeaveTimeForm3"]);//int.Parse(LeaveTime.Text);
            accset.LeaveMode = Request.Params["ddlleavemodeForm3"] == null ? SeatManage.EnumType.EnterOutLogType.Leave : (SeatManage.EnumType.EnterOutLogType)int.Parse(Request.Params["ddlleavemodeForm3"]);// SeatManage.EnumType.EnterOutLogType.ShortLeave; //(SeatManage.EnumType.EnterOutLogType)int.Parse(ddlleavemode.SelectedValue);
            accset.IsReleaseOnSeat = Request.Params["IsOnSeat"] == null ? false : true; //IsOnSeat.Checked;
            accset.IsComeBack = Request.Params["IsShortLeave"] == null ? false : true; //IsShortLeave.Checked;
            accset.IsBookingConfinmed = Request.Params["IsBooking"] == null ? false : true; //IsBooking.Checked;
            if (SeatManage.Bll.T_SM_SystemSet.UpdateAccessSetting(accset))
            {
                result = Json(new { status = "yes", message = "门禁联动规则配置保存成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = Json(new { status = "no", message = "门禁联动规则配置保存成功" }, JsonRequestBehavior.AllowGet);
            }

            return result;
        }

        public JsonResult SaveBlacklistInfoSetting()
        {
            JsonResult result = null;

            SeatManage.ClassModel.RegulationRulesSetting rulessetting = new SeatManage.ClassModel.RegulationRulesSetting();

            rulessetting.BlacklistSet.Used = Request.Params["IsBlUserd"] == null ? false : true;// IsBlUserd.Checked;
            rulessetting.BlacklistSet.ViolateTimes = Request.Params["nbvrcont"] == null ? 3 : int.Parse(Request.Params["nbvrcont"].Trim()); //(nbvrcont.Text);

            rulessetting.BlacklistSet.LeaveBlacklist = Request.Params["ddlleavemode"] == null ? SeatManage.EnumType.LeaveBlacklistMode.AutomaticMode : (SeatManage.EnumType.LeaveBlacklistMode)int.Parse(Request.Params["ddlleavemode"].ToString());//Request.Params["ddlleavemode"]==null? SeatManage.EnumType.LeaveBlacklistMode.AutomaticMode: (SeatManage.EnumType.LeaveBlacklistMode)int.Parse(Request.Params["ddlleavemode"]);//(ddlleavemode.SelectedValue);

            rulessetting.BlacklistSet.LimitDays = Request.Params["nbleavetime"] == null ? 7 : int.Parse(Request.Params["nbleavetime"].Trim());//(nbleavetime.Text);
            rulessetting.BlacklistSet.ViolateFailDays = Request.Params["nbvrovertime"] == null ? 30 : int.Parse(Request.Params["nbvrovertime"].Trim());//(nbvrovertime.Text);

            rulessetting.BlacklistSet.ViolateRoule[SeatManage.EnumType.ViolationRecordsType.BookingTimeOut] = Request.Params["cbBookOverTime"] == null ? false : true;//cbBookOverTime.Checked;
            rulessetting.BlacklistSet.ViolateRoule[SeatManage.EnumType.ViolationRecordsType.LeaveByAdmin] = Request.Params["cbLeaveByAdmin"] == null ? false : true;//cbLeaveByAdmin.Checked;
            rulessetting.BlacklistSet.ViolateRoule[SeatManage.EnumType.ViolationRecordsType.SeatOutTime] = Request.Params["cbSeatOverTime"] == null ? false : true; //cbSeatOverTime.Checked;
            rulessetting.BlacklistSet.ViolateRoule[SeatManage.EnumType.ViolationRecordsType.ShortLeaveByAdminOutTime] = Request.Params["cbShortLeaveByAdmin"] == null ? false : true; //cbShortLeaveByAdmin.Checked;
            rulessetting.BlacklistSet.ViolateRoule[SeatManage.EnumType.ViolationRecordsType.ShortLeaveByReaderOutTime] = Request.Params["cbShortLeaveByReader"] == null ? false : true; //cbShortLeaveByReader.Checked;
            rulessetting.BlacklistSet.ViolateRoule[SeatManage.EnumType.ViolationRecordsType.ShortLeaveOutTime] = Request.Params["cbShortLeaveOverTime"] == null ? false : true;// cbShortLeaveOverTime.Checked;

            if (SeatManage.Bll.T_SM_SystemSet.UpdateRegulationRulesSetting(rulessetting))
            {
                result = Json(new { status = "yes", message = "黑名单规则配置保存成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = Json(new { status = "no", message = "黑名单规则配置保存失败" }, JsonRequestBehavior.AllowGet);
            }

            return result;
        }

        public JsonResult SavePecketWebSetting()
        {
            JsonResult result = null;
            SeatManage.ClassModel.PecketBookWebSetting setting = new SeatManage.ClassModel.PecketBookWebSetting();
            setting.UseBookComfirm = Request.Params["cb_UseBookComfirm"] == null ? false : true;// cb_UseBookComfirm.Checked;
            setting.UseBookNextDaySeat = Request.Params["cb_UseBookNextDaySeat"] == null ? false : true;//cb_UseBookNextDaySeat.Checked;
            setting.UseBookNowDaySeat = Request.Params["cb_UseBookNowDaySeat"] == null ? false : true;//cb_UseBookNowDaySeat.Checked;
            setting.UseBookSeat = Request.Params["cb_UseBookSeat"] == null ? false : true;//cb_UseBookSeat.Checked;
            setting.UseCancelBook = Request.Params["cb_UseCancelBook"] == null ? false : true;//cb_UseCancelBook.Checked;
            setting.UseCancelWait = Request.Params["cb_UseCancelWait"] == null ? false : true;//cb_UseCancelWait.Checked;
            setting.UseCanLeave = Request.Params["cb_UseCanLeave"] == null ? false : true;//cb_UseCanLeave.Checked;
            setting.UseComeBack = Request.Params["cb_UseComeBack"] == null ? false : true;//cb_UseComeBack.Checked;
            setting.UseContinue = Request.Params["cb_UseContinue"] == null ? false : true;// cb_UseContinue.Checked;
            setting.UseShortLeave = Request.Params["cb_UseShortLeave"] == null ? false : true;// cb_UseShortLeave.Checked;
            setting.UseWaitSeat = Request.Params["cb_UseWaitSeat"] == null ? false : true;//cb_UseWaitSeat.Checked;
            setting.UseSelectSeat = Request.Params["cb_SelectSeat"] == null ? false : true;//cb_SelectSeat.Checked;
            setting.UseChangeSeat = Request.Params["cb_ChangeSeat"] == null ? false : true;//cb_ChangeSeat.Checked;

            if (SeatManage.Bll.T_SM_SystemSet.UpdatePecketWebSetting(setting))
            {
                result = Json(new { status = "yes", message = "微信规则配置保存成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = Json(new { status = "no", message = "微信规则配置保存失败" }, JsonRequestBehavior.AllowGet);
            }
            return result;
        }

        public JsonResult SavePushMsgSetting()
        {
            JsonResult result = null;

            SeatManage.ClassModel.PushMsssageSetting setting = new SeatManage.ClassModel.PushMsssageSetting();
            setting.PushSetting[SeatManage.EnumType.MsgPushType.AdminOperation] = Request.Params["cb_AdminOperation"] == null ? false : true; //cb_AdminOperation.Checked;
            setting.PushSetting[SeatManage.EnumType.MsgPushType.EnterVR] = Request.Params["cb_EnterVr"] == null ? false : true; // cb_EnterVr.Checked;
            setting.PushSetting[SeatManage.EnumType.MsgPushType.EnterBlack] = Request.Params["cb_EnterBlack"] == null ? false : true; // cb_EnterBlack.Checked;
            setting.PushSetting[SeatManage.EnumType.MsgPushType.LeaveVrBlack] = Request.Params["cb_LeaveVrBlack"] == null ? false : true; // cb_LeaveVrBlack.Checked;
            setting.PushSetting[SeatManage.EnumType.MsgPushType.OtherUser] = Request.Params["cb_OtherUser"] == null ? false : true; // cb_OtherUser.Checked;
            setting.PushSetting[SeatManage.EnumType.MsgPushType.TimeOut] = Request.Params["cb_TimeOut"] == null ? false : true; //cb_TimeOut.Checked;
            setting.PushSetting[SeatManage.EnumType.MsgPushType.UserOperation] = Request.Params["cb_UserOperation"] == null ? false : true; // cb_UserOperation.Checked;
            if (SeatManage.Bll.T_SM_SystemSet.SaveMsgPushSet(setting))
            {
                result = Json(new { status = "yes", message = "消息推送规则配置保存成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = Json(new { status = "no", message = "消息推送规则配置保存失败" }, JsonRequestBehavior.AllowGet);
            }

            return result;
        }

        public ActionResult BlacklistInfo()
        {
            //初始化黑名单设置页面
            SeatManage.ClassModel.BlacklistSetting blacklistset = SeatManage.Bll.T_SM_SystemSet.GetRegulationRulesSetting().BlacklistSet;
            ViewBag.IsBlUserdChecked = blacklistset.Used ? "yes" : "no";
            ViewBag.nbvrcontText = blacklistset.ViolateTimes.ToString();
            ViewBag.ddlleavemodeSelectedValue = ((int)blacklistset.LeaveBlacklist).ToString();
            ViewBag.nbleavetimeText = blacklistset.LimitDays.ToString();
            ViewBag.nbvrovertimeText = blacklistset.ViolateFailDays.ToString();
            ViewBag.cbBookOverTimeChecked = blacklistset.ViolateRoule[SeatManage.EnumType.ViolationRecordsType.BookingTimeOut] ? "yes" : "no";
            ViewBag.cbLeaveByAdminChecked = blacklistset.ViolateRoule[SeatManage.EnumType.ViolationRecordsType.LeaveByAdmin] ? "yes" : "no";
            ViewBag.cbSeatOverTimeChecked = blacklistset.ViolateRoule[SeatManage.EnumType.ViolationRecordsType.SeatOutTime] ? "yes" : "no";
            ViewBag.cbShortLeaveByAdminChecked = blacklistset.ViolateRoule[SeatManage.EnumType.ViolationRecordsType.ShortLeaveByAdminOutTime] ? "yes" : "no";
            ViewBag.cbShortLeaveByReaderChecked = blacklistset.ViolateRoule[SeatManage.EnumType.ViolationRecordsType.ShortLeaveByReaderOutTime] ? "yes" : "no";
            ViewBag.cbShortLeaveOverTimeChecked = blacklistset.ViolateRoule[SeatManage.EnumType.ViolationRecordsType.ShortLeaveOutTime] ? "yes" : "no";

            //初始化门禁设置页面
            SeatManage.ClassModel.AccessSetting accset = SeatManage.Bll.T_SM_SystemSet.GetAccessSetting();
            if (accset == null)
            {
                accset = new SeatManage.ClassModel.AccessSetting();
            }

            ViewBag.IsASUserdCheckedForm3 = accset.IsUsed ? "yes" : "no";
            ViewBag.IsELUserdCheckedForm3 = accset.EnterLib ? "yes" : "no";
            ViewBag.IsOLUserdCheckedForm3 = accset.OutLib ? "yes" : "no";
            ViewBag.cbBLIsUsedCheckedForm3 = accset.IsLimitBlackList ? "yes" : "no";
            ViewBag.IsAddrvCheckedForm3 = accset.AddViolationRecords ? "yes" : "no";
            ViewBag.LeaveTimeTextForm3 = accset.LeaveTimeSpan.ToString();
            ViewBag.ddlleavemodeSelectedValueForm3 = ((int)accset.LeaveMode).ToString();
            ViewBag.IsOnSeatCheckedForm3 = accset.IsReleaseOnSeat ? "yes" : "no";
            ViewBag.IsShortLeaveCheckedForm3 = accset.IsComeBack ? "yes" : "no";
            ViewBag.IsBookingCheckedForm3 = accset.IsBookingConfinmed ? "yes" : "no";
            //初始化微信端设置
            SeatManage.ClassModel.PecketBookWebSetting setting = SeatManage.Bll.T_SM_SystemSet.GetPecketWebSetting();
            if (setting == null)
            {
                setting = new SeatManage.ClassModel.PecketBookWebSetting();
            }
            ViewBag.cb_UseBookComfirmChecked = setting.UseBookComfirm;
            ViewBag.cb_UseBookNextDaySeatChecked = setting.UseBookNextDaySeat;
            ViewBag.cb_UseBookNowDaySeatChecked = setting.UseBookNowDaySeat;
            ViewBag.cb_UseBookSeatChecked = setting.UseBookSeat;
            ViewBag.cb_UseCancelBookChecked = setting.UseCancelBook;
            ViewBag.cb_UseCancelWaitChecked = setting.UseCancelWait;
            ViewBag.cb_UseCanLeaveChecked = setting.UseCanLeave;
            ViewBag.cb_UseComeBackChecked = setting.UseComeBack;
            ViewBag.cb_UseContinueChecked = setting.UseContinue;
            ViewBag.cb_UseShortLeaveChecked = setting.UseShortLeave;
            ViewBag.cb_UseWaitSeatChecked = setting.UseWaitSeat;
            ViewBag.cb_ChangeSeatChecked = setting.UseChangeSeat;
            ViewBag.cb_SelectSeatChecked = setting.UseSelectSeat;
            //初始化消息推送
            SeatManage.ClassModel.PushMsssageSetting objPushMsssageSetting = SeatManage.Bll.T_SM_SystemSet.GetMsgPushSet() ?? new SeatManage.ClassModel.PushMsssageSetting();
            ViewBag.cb_AdminOperationChecked = objPushMsssageSetting.PushSetting[SeatManage.EnumType.MsgPushType.AdminOperation];
            ViewBag.cb_EnterVrChecked = objPushMsssageSetting.PushSetting[SeatManage.EnumType.MsgPushType.EnterVR];
            ViewBag.cb_EnterBlackChecked = objPushMsssageSetting.PushSetting[SeatManage.EnumType.MsgPushType.EnterBlack];
            ViewBag.cb_LeaveVrBlackChecked = objPushMsssageSetting.PushSetting[SeatManage.EnumType.MsgPushType.LeaveVrBlack];
            ViewBag.cb_OtherUserChecked = objPushMsssageSetting.PushSetting[SeatManage.EnumType.MsgPushType.OtherUser];
            ViewBag.cb_TimeOutChecked = objPushMsssageSetting.PushSetting[SeatManage.EnumType.MsgPushType.TimeOut];
            ViewBag.cb_UserOperationChecked = objPushMsssageSetting.PushSetting[SeatManage.EnumType.MsgPushType.UserOperation];
            return View();
        }


        public ActionResult SyncSet()
        {
            return View();
        }
        public ActionResult AccessSetting()
        {
            return View();
        }
        public ActionResult PecketWebSetting()
        {
            return View();
        }
        public ActionResult PushMsgSetting()
        {
            return View();
        }
    }
}