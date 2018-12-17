using System;
using System.Collections.Generic;
using System.Linq;
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



        public ActionResult DeviceInfo()
        {
            return View();
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
                result = Json(new { status = "yes", message = "黑名单规则配置保存失败" }, JsonRequestBehavior.AllowGet);
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