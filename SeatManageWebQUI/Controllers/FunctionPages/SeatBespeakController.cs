using Newtonsoft.Json;
using SeatManage.Bll;
using SeatManage.ClassModel;
using SeatManage.EnumType;
using SeatManageWebQUI.Controllers.FunctionPages.Code;
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
    public class SeatBespeakController : BaseController
    {
        // GET: SeatBespeak
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SeatOp(string opFlag)
        {
            JsonResult jsonResult = null;
            if (opFlag == "delay")
            {
                SeatManage.ClassModel.ReaderInfo reader = EnterOutOperate.GetReaderInfo(this.LoginId);
                reader.EnterOutLog.Remark = "通过预约网站延长座位使用时间";
                reader.EnterOutLog.EnterOutState = EnterOutLogType.ContinuedTime;
                string result = EnterOutOperate.DelaySeatUsedTime(reader);
                if (!string.IsNullOrEmpty(result))
                {
                    jsonResult = Json(new { status = "no", message = "操作出错，请重新尝试" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    jsonResult = Json(new { status = "yes", message = "通过预约网站延长座位使用时间,操作成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (opFlag == "shortLeave")
            {
                try
                {
                    SeatManage.ClassModel.ReaderInfo reader = EnterOutOperate.GetReaderInfo(this.LoginId);
                    int newLogId = -1;
                    reader.EnterOutLog.EnterOutState = EnterOutLogType.ShortLeave;
                    reader.EnterOutLog.Flag = Operation.Reader;
                    reader.EnterOutLog.Remark = "通过预约网站设置暂离";
                    HandleResult reault = EnterOutOperate.AddEnterOutLog(reader.EnterOutLog, ref newLogId);
                    if (reault == HandleResult.Successed)
                    {
                      //  lblMsg.Text = "操作成功";
                        jsonResult = Json(new { status = "yes", message = "通过预约网站设置暂离,操作成功" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    jsonResult = Json(new { status = "no", message = "操作失败，请重新尝试" }, JsonRequestBehavior.AllowGet);
                }

            }
            else if(opFlag== "leave")
            {
                try
                {
                    SeatManage.ClassModel.ReaderInfo reader = EnterOutOperate.GetReaderInfo(this.LoginId);
                    int newLogId = -1;
                    reader.EnterOutLog.EnterOutState = EnterOutLogType.Leave;
                    reader.EnterOutLog.Flag = Operation.Reader;
                    reader.EnterOutLog.Remark = "通过预约网站释放座位";
                    HandleResult reault = EnterOutOperate.AddEnterOutLog(reader.EnterOutLog, ref newLogId);
                    if (reault == HandleResult.Successed)
                    {
                        jsonResult = Json(new { status = "yes", message = "通过预约网站释放座位，操作成功" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    jsonResult = Json(new { status = "no", message = "操作失败，请重新尝试" }, JsonRequestBehavior.AllowGet);
                }
            }
            return jsonResult;
        }



        public ActionResult BespeakSelfSeat()
        {
            SeatManage.ClassModel.ReaderInfo reader = EnterOutOperate.GetReaderInfo(this.LoginId);
            if (reader.AtReadingRoom == null)
            {
                ViewBag.btnDelayTimeEnabled = "false";
                ViewBag.btnLeaveEnabled = "false";
                ViewBag.btnShortLeaveEnabled = "false";

                ViewBag.lblHoldTimeText = "暂无";
                ViewBag.lblStateText = "无座";
                ViewBag.lblSeatInfoText = "您目前是无座状态，请预约或者选座";
            }
            else
            {
                SeatManage.ClassModel.ReadingRoomSetting roomSet = reader.AtReadingRoom.Setting;
                if (roomSet.SeatUsedTimeLimit.Used && roomSet.SeatUsedTimeLimit.IsCanContinuedTime)
                {
                    ViewBag.btnDelayTimeEnabled = roomSet.SeatBespeak.AllowDelayTime ? "true" : "false";
                }

                ViewBag.btnLeaveEnabled = roomSet.SeatBespeak.AllowLeave? "true" : "false";
                ViewBag.btnShortLeaveEnabled = roomSet.SeatBespeak.AllowShortLeave? "true" : "false"; 


                if (reader.EnterOutLog != null && reader.EnterOutLog.EnterOutState != EnterOutLogType.Leave)
                {
                    switch (reader.EnterOutLog.EnterOutState)
                    {
                        case EnterOutLogType.ShortLeave:
                            ViewBag.lblSeatInfoText = string.Format("{0} {1}", reader.EnterOutLog.ReadingRoomName, reader.EnterOutLog.ShortSeatNo);
                            ViewBag.lblStateText = "暂离";
                            ViewBag.lblHoldTimeText = reader.EnterOutLog.EnterOutTime.ToString();
                            ViewBag.btnShortLeaveEnabled = "false";
                            ViewBag.btnDelayTimeEnabled = "false";
                            break;
                        case EnterOutLogType.BookingConfirmation:
                        case EnterOutLogType.SelectSeat:
                        case EnterOutLogType.ContinuedTime:
                        case EnterOutLogType.ComeBack:
                        case EnterOutLogType.ReselectSeat:
                        case EnterOutLogType.WaitingSuccess:
                            ViewBag.lblSeatInfoText = string.Format("{0} {1}", reader.EnterOutLog.ReadingRoomName, reader.EnterOutLog.ShortSeatNo);
                            ViewBag.lblStateText = "在座";
                            ViewBag.lblHoldTimeText = reader.EnterOutLog.EnterOutTime.ToString();
                            break;
                        default:
                            ViewBag.btnDelayTimeEnabled = "false";
                            ViewBag.btnLeaveEnabled = "false";
                            ViewBag.btnShortLeaveEnabled = "false";
                            break;
                    }
                }
                else
                {
                    ViewBag.lblSeatInfoText = "无";
                    ViewBag.btnDelayTimeEnabled = "false";
                    ViewBag.btnLeaveEnabled = "false";
                    ViewBag.btnShortLeaveEnabled = "false";
                }
            }
            return View();
        }

        #region 隔天
        public JsonResult CheckBooking(string SelectedDate, string roomNo, string canBespeakAmountStr)
        {
            JsonResult result = null;
            DateTime nowDate = SeatManage.Bll.ServiceDateTime.Now;
            DateTime selectedDate = DateTime.Parse(SelectedDate);//this.dpStartDate.SelectedDate.Value;
            ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(roomNo);
            string roomSet = room.Setting.ToString();
            if (string.IsNullOrEmpty(roomSet))
            {
                result = Json(new { status = "no", message = "该阅览室没有配置" }, JsonRequestBehavior.AllowGet);
            }
            else if (selectedDate.CompareTo(nowDate) <= 0)
            {
                result = Json(new { status = "no", message = "日期选择错误" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    SeatManage.ClassModel.ReadingRoomSetting set = new SeatManage.ClassModel.ReadingRoomSetting(roomSet);
                    int canBespeakAmount = int.Parse(canBespeakAmountStr);
                    if (!set.SeatBespeak.Used || set.RoomOpenSet.UninterruptibleModel)
                    {
                        result = Json(new { status = "no", message = "阅览室没有开放预约" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (!dateBespeak(set.SeatBespeak, nowDate, SelectedDate))
                    {
                        result = Json(new { status = "no", message = "该日期座位不能预约" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (!timeCanBespeak(set.SeatBespeak, nowDate))
                    {
                        result = Json(new { status = "no", message = string.Format("预约时间为：{0}到{1}", set.SeatBespeak.CanBespeatTimeSpace.BeginTime, set.SeatBespeak.CanBespeatTimeSpace.EndTime) }, JsonRequestBehavior.AllowGet);
                    }
                    else if (canBespeakAmount <= 0)
                    {
                        result = Json(new { status = "no", message = "座位已被全部预约" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        string urlParametersStr = SeatManage.SeatManageComm.AESAlgorithm.DESEncode(string.Format("roomNo={0}&date={1}", roomNo, selectedDate.ToBinary()));
                        result = Json(new { status = "yes", message = "预约座位", urlParameters = urlParametersStr }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {

                    result = Json(new { status = "no", message = ex.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            //  var jsonstr = JsonConvert.SerializeObject(result);
            return result;
        }

        protected string bespeakDate = "";
        protected string roomNum = "";

        public ActionResult BespeakSeatLayout()
        {

            if (!OpVerifiction())
            {
                Response.Write("<html><head><title>系统安全提示</title><script>alert('请使用正常方式访问网站');location.href='/seat/Login'</script></head><body></body></html>");
                Response.End();
            }
            else if (string.IsNullOrEmpty(Request.QueryString["Param"]))
            {
                Response.Write("<html><head><title>系统安全提示</title><script>alert('你的操作不合法，请使用正确途径预约座位');location.href='/seat/Login'</script></head><body></body></html>");
                Response.End();
            }
            else
            {
                string Param = Request.Params["Param"].ToString();
                BespeakSubmitWindowParamModel par = new BespeakSubmitWindowParamModel(Request.QueryString["Param"]);
                roomNum = par.RoomNo;
                try
                {
                    bespeakDate = DateTime.FromBinary(long.Parse(par.BespeakDate)).ToString();
                }
                catch
                {
                    Response.Write("<html><head><title>系统安全提示</title><script>alert('你的操作不合法，请使用正确途径预约座位');location.href='/Login'</script></head><body></body></html>");
                    //FineUI.Alert.Show("预约日期不正确");
                    Response.End();
                }
                ViewBag.bespeakDate = bespeakDate;
                ViewBag.roomNum = roomNum;
            }

            return View();
        }

        /// <summary>
        /// 校验指定房间能预约
        /// </summary>
        /// <param name="set"></param>
        /// <param name="nowDate"></param>
        /// <param name="SelectedDate"></param>
        /// <returns></returns>
        private bool dateBespeak(SeatManage.ClassModel.SeatBespeakSet set, DateTime nowDate, string SelectedDate)
        {
            DateTime selectedDate = DateTime.Parse(SelectedDate);//this.dpStartDate.SelectedDate.Value;
            TimeSpan span = selectedDate.Date - nowDate.Date;
            //判断当天是否大于选择的日期
            if (span.Days > set.BespeakBeforeDays)
            {
                return false;
            }
            for (int i = 0; i < set.NoBespeakDates.Count; i++)
            {
                try
                {
                    DateTime beginDate = DateTime.Parse(set.NoBespeakDates[i].BeginTime);
                    DateTime endDate = DateTime.Parse(set.NoBespeakDates[i].EndTime);
                    if (SeatManage.SeatManageComm.DateTimeOperate.DateAccord(beginDate, endDate, selectedDate) || selectedDate.CompareTo(beginDate) == 0 || selectedDate.CompareTo(endDate) == 0)
                    {//如果当前时间符合某个不可预约的规则，则直接返回false，不可预约
                        return false;
                    }
                }
                catch
                {//日期转换遇到异常，则忽略 
                }
            }
            return true;
        }

        /// <summary>
        /// 校验指定日期日期是否能预约
        /// </summary>
        /// <param name="set"></param>
        /// <param name="nowDate"></param>
        /// <returns></returns>
        private bool timeCanBespeak(SeatManage.ClassModel.SeatBespeakSet set, DateTime nowDate)
        {
            try
            {
                DateTime beginTime = DateTime.Parse(string.Format("{0} {1}", nowDate.ToShortDateString(), set.CanBespeatTimeSpace.BeginTime));
                DateTime endTime = DateTime.Parse(string.Format("{0} {1}", nowDate.ToShortDateString(), set.CanBespeatTimeSpace.EndTime));
                if (SeatManage.SeatManageComm.DateTimeOperate.DateAccord(beginTime, endTime, nowDate))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        /// <summary>
        /// 判断座位是否符合预约条件
        /// </summary>
        /// <returns></returns>
        protected bool IsCanBespeak(string roomNo, string selDate)
        {
            try
            {
                bool result = true;
                DateTime nowDate = SeatManage.Bll.ServiceDateTime.Now;
                SeatManage.ClassModel.ReadingRoomSetting set = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(roomNo).Setting;
                if (!set.SeatBespeak.Used)
                {
                    Response.Write("<html><head><title>系统安全提示</title><script>alert('阅览室没有开放预约');location.href='/seat/Login'</script></head><body></body></html>");
                    Response.End();
                }
                if (!dateBespeak(set.SeatBespeak, nowDate, selDate))
                {
                    Response.Write("<html><head><title>系统安全提示</title><script>alert('该日期不能预约');location.href='/seat/Login'</script></head><body></body></html>");
                    Response.End();
                }
                if (!timeCanBespeak(set.SeatBespeak, nowDate))
                {
                    Response.Write("<html><head><title>系统安全提示</title><script>alert('" + string.Format("预约时间为：{0}到{1}", set.SeatBespeak.CanBespeatTimeSpace.BeginTime, set.SeatBespeak.CanBespeatTimeSpace.EndTime) + "');location.href='/seat/Login'</script></head><body></body></html>");
                    Response.End();
                }
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        /// <summary>
        /// 绘制预约座位图
        /// </summary>
        /// <param name="roomNum"></param>
        /// <param name="date"></param>
        /// <param name="divTransparentTop"></param>
        /// <param name="divTransparentLeft"></param>
        /// <returns></returns>
        public string drowBespeakSeatLayOutHtml(string roomNum, string date, string divTransparentTop, string divTransparentLeft)
        {
            if (!IsCanBespeak(roomNum, date))
            {
                WriteLogs("阅览室布局页面");
                Response.Write("<html><head><title>系统安全提示</title><script>alert('你的操作不合法，请使用正确途径预约座位');location.href='/seat/Login'</script></head><body></body></html>");
                Response.End();
            }

            return new SeatLayoutTools().drowBespeakSeatLayOutHtml(roomNum, date, divTransparentTop, divTransparentLeft);
        }

        public string BindBespeakModel2TimeSelect(string seatNo, string seatShortNo, string dateString, string roomNo)
        {
            SeatManage.ClassModel.ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(roomNo);
            SeatManage.Bll.T_SM_ReadingRoom bllReadingRoom = new SeatManage.Bll.T_SM_ReadingRoom();
            StringBuilder sb = new StringBuilder();
            DateTime date = DateTime.Parse(dateString);
            DateTime minTime = DateTime.Parse(date.ToShortDateString() + " " + bllReadingRoom.GetRoomOpenTimeByDate(room.Setting, date.ToShortDateString()).BeginTime);
            sb.Append("<select onchange=\"selChange()\" prompt=\"请选择\" id=\"timeSelect\">");
            while (true)
            {
                minTime = minTime.AddMinutes(10);
                if (minTime.Date > date.Date)
                {
                    break;
                }
                if (SeatManageWebV5.Code.NowReadingRoomState.ReadingRoomOpenState(room.Setting.RoomOpenSet, minTime) == SeatManage.EnumType.ReadingRoomStatus.Close)
                {
                    continue;
                }
                sb.Append("<option value=\"" + minTime.ToShortTimeString() + "\">" + minTime.ToShortTimeString() + "</option>");
            }

            sb.Append("</select>");

            string roomOpenTime = bllReadingRoom.GetRoomOpenTimeByDate(room.Setting, DateTime.Parse(dateString).ToShortDateString()).BeginTime;
            ViewBag.lblEndDate = bespeakSureTimeSpan(room.Setting, "1", dateString);
            return sb.ToString(); ;
        }

        public string SelectTimeChange(string roomNo, string selValue)
        {
            SeatManage.ClassModel.ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(roomNo);
            DateTime bespeakTime = DateTime.Parse(selValue);
            DateTime bespeakBeginTime = bespeakTime.AddMinutes(-double.Parse(room.Setting.SeatBespeak.ConfirmTime.BeginTime));
            DateTime bespeakEndTime = bespeakTime.AddMinutes(double.Parse(room.Setting.SeatBespeak.ConfirmTime.EndTime));
            return string.Format("{0}至{1}", bespeakBeginTime.ToShortTimeString(), bespeakEndTime.ToShortTimeString());
        }

        /// <summary>
        /// 确认按钮
        /// </summary>
        /// <returns></returns>
        public string btnBespeakSubmit(string rblModelSelectedValue, string date, string roomOpenTimeValue, string TimeSelectedText, string roomNo, string seatNo)
        {
            SeatManage.ClassModel.BespeakLogInfo bespeakModel = new SeatManage.ClassModel.BespeakLogInfo();
            bespeakModel.BsepeakState = SeatManage.EnumType.BookingStatus.Waiting;
            DateTime bespeatDate = DateTime.Parse(string.Format("{0} {1}", DateTime.Parse(date).ToShortDateString(), roomOpenTimeValue));
            if (rblModelSelectedValue == "1")
            {
                bespeatDate = DateTime.Parse(string.Format("{0} {1}", DateTime.Parse(date).ToShortDateString(), TimeSelectedText));
            }
            bespeakModel.BsepeakTime = bespeatDate;
            bespeakModel.CardNo = this.LoginId;
            bespeakModel.ReadingRoomNo = roomNo.Trim();
            bespeakModel.Remark = string.Format("读者通过Web页面预约座位");
            bespeakModel.SeatNo = seatNo;
            bespeakModel.SubmitTime = SeatManage.Bll.ServiceDateTime.Now;
            JsonResult jsonResult = null;
            try
            {
                SeatManage.ClassModel.ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(bespeakModel.ReadingRoomNo);
                if (!room.Setting.IsCanBespeakSeat(bespeakModel.BsepeakTime))
                {
                    jsonResult = Json(new { status = "no", message = "对不起不能预约当前日期的座位，请刷新页面重新选择日期。" }, JsonRequestBehavior.AllowGet);
                }
                if (bespeakModel.SubmitTime > bespeakModel.BsepeakTime)
                {
                    jsonResult = Json(new { status = "no", message = "对不起不能预约时间错误，请刷新页面重新选择日期。" }, JsonRequestBehavior.AllowGet);
                }
                if (room.Setting.SeatBespeak.BespeakArea.BespeakType == BespeakAreaType.Percentage)
                {
                    List<BespeakLogInfo> bespeaklogs = SeatManage.Bll.T_SM_SeatBespeak.GetNotCheckedBespeakLogInfo(new List<string>() { bespeakModel.ReadingRoomNo }, bespeakModel.BsepeakTime);
                    double canbookCount = (double)((double)(room.SeatList.Seats.Count - room.SeatList.Seats.Where(u => u.Value.IsSuspended).ToArray().Count()) * room.Setting.SeatBespeak.BespeakArea.Scale);
                    if (bespeaklogs.Count >= canbookCount)
                    {
                        jsonResult = Json(new { status = "no", message = "对不起当前阅览室已经没有可预约的座位。" }, JsonRequestBehavior.AllowGet);
                    }
                }

                SeatManage.EnumType.HandleResult result = SeatManage.Bll.T_SM_SeatBespeak.AddBespeakLogInfo(bespeakModel);
                if (result == SeatManage.EnumType.HandleResult.Successed)
                {
                    jsonResult = Json(new { status = "yes", message = "座位预约成功，请在规定的时间内刷卡确认。" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    jsonResult = Json(new { status = "no", message = "预约失败，该座位已经被别人预约。" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                jsonResult = Json(new { status = "no", message = string.Format("执行预约操作遇到错误：{0}", ex.Message) }, JsonRequestBehavior.AllowGet);
            }
            var jsonstr = JsonConvert.SerializeObject(jsonResult);
            return jsonstr;
        }

        /// <summary>
        ///打开预约确认端口
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public ActionResult BespeakSubmitWindow(string parm)
        {
            if (Request.ServerVariables["HTTP_REFERER"] != null)
            {
                string url = Request.ServerVariables["HTTP_REFERER"].Trim();
                string pageName = SeatManage.SeatManageComm.SeatComm.GetPageName(url);
                if (pageName.ToUpper() != "INDEX")// && pageName != "FormSYS.aspx")
                {
                    WriteLogs("阅览室布局页面");
                    Response.Write("<html><head><title>系统安全提示</title><script>alert('请通过正确方式访问网站');location.href='/Login'</script></head><body></body></html>");
                    Response.End();
                }
            }
            else
            {
                WriteLogs("阅览室布局页面");
                WriteLogs(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                Response.Write("<html><head><title>系统安全提示</title><script>alert('请通过正确方式访问网站');location.href='/Login'</script></head><body></body></html>");
                Response.End();
            }
            SeatManageWebV5.Code.BespeakSubmitWindowParamModel bespeakSubmitModel = new BespeakSubmitWindowParamModel(parm);
            string seatNo = bespeakSubmitModel.SeatNo;
            string seatShortNo = bespeakSubmitModel.ShortSeatNo;
            string date = DateTime.FromBinary(long.Parse(bespeakSubmitModel.BespeakDate)).ToString();
            string roomNo = bespeakSubmitModel.RoomNo;
            DateTime nowDate = SeatManage.Bll.ServiceDateTime.Now;

            if (!IsCanBespeak(roomNo, date))
            {
                WriteLogs("阅览室布局页面");
                Response.Write("<html><head><title>系统安全提示</title><script>alert('请通过正确方式访问网站');</script></head><body></body></html>");
                Response.End();
            }
            SeatManage.ClassModel.ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(roomNo);
            List<SeatManage.ClassModel.BespeakLogInfo> list = SeatManage.Bll.T_SM_SeatBespeak.GetBespeakLogInfoBySeatNo(seatNo, DateTime.Parse(date));
            SeatManage.Bll.T_SM_ReadingRoom bllReadingRoom = new SeatManage.Bll.T_SM_ReadingRoom();
            string roomOpenTime = bllReadingRoom.GetRoomOpenTimeByDate(room.Setting, DateTime.Parse(date).ToShortDateString()).BeginTime;

            //判断自己是否已经预约座位
            List<SeatManage.EnumType.BookingStatus> bespeakStatus = new List<SeatManage.EnumType.BookingStatus>();
            bespeakStatus.Add(SeatManage.EnumType.BookingStatus.Waiting);
            List<SeatManage.ClassModel.BespeakLogInfo> readerBespeaklist = SeatManage.Bll.T_SM_SeatBespeak.GetBespeakLogInfoByCardNo(this.LoginId, DateTime.Parse(date));//.GetBespeakList(this.LoginId, null, date, 0, bespeakStatus);
            if (readerBespeaklist.Count > 0)
            {
                //top.Dialog.close();

                Response.Write("<html><head><title>系统提示</title><script>alert('您选择的日期已经预约了座位，请先取消原来的预约。');top.Dialog.close();</script></head><body></body></html>");
                Response.End();
            }
            //判断座位是否被别人预约
            List<SeatManage.ClassModel.BespeakLogInfo> listBespeakLogInfo = SeatManage.Bll.T_SM_SeatBespeak.GetBespeakLogInfoBySeatNo(seatNo, DateTime.Parse(date));

            //roomOpenTime = bllReadingRoom.GetRoomOpenTimeByDate(room.Setting, DateTime.Parse(date).ToShortDateString()).BeginTime;
            // this.lblEndDate.Text = bespeakSureTimeSpan(room.Setting);
            for (int i = 0; i < listBespeakLogInfo.Count; i++)
            {
                if (list[i].BsepeakState == SeatManage.EnumType.BookingStatus.Waiting)
                {
                    Response.Write("<html><head><title>系统提示</title><script>alert('座位已经被别人预约，请预约其他座位');top.Dialog.close();</script></head><body></body></html>");
                    Response.End();
                }
            }
            //判断是否已加入黑名单
            List<SeatManage.ClassModel.BlackListInfo> blacklistInfoByCardNo = SeatManage.Bll.T_SM_Blacklist.GetBlackListInfo(this.LoginId);
            SeatManage.ClassModel.RegulationRulesSetting rulesSet = SeatManage.Bll.T_SM_SystemSet.GetRegulationRulesSetting();
            if (room.Setting.UsedBlacklistLimit && blacklistInfoByCardNo.Count > 0)
            {
                if (room.Setting.BlackListSetting.Used)
                {
                    bool isblack = false;
                    foreach (SeatManage.ClassModel.BlackListInfo blinfo in blacklistInfoByCardNo)
                    {
                        if (blinfo.ReadingRoomID == room.No)
                        {
                            isblack = true;
                            break;
                        }
                    }
                    if (isblack)
                    {
                        Response.Write("<html><head><title>系统提示</title><script>alert('你已进入黑名单，不能在该阅览室预约座位');top.Dialog.close();</script></head><body></body></html>");
                        Response.End();
                    }
                }
                else
                {
                    Response.Write("<html><head><title>系统提示</title><script>alert('你已进入黑名单，不能在该阅览室预约座位');top.Dialog.close();</script></head><body></body></html>");
                    Response.End();
                }
            }
            if (room.Setting.LimitReaderEnter.Used)
            {
                SeatManage.ClassModel.ReaderInfo readerInfo = SeatManage.Bll.EnterOutOperate.GetReaderInfo(this.LoginId);
                string[] litype = room.Setting.LimitReaderEnter.ReaderTypes.Split(';');
                if (!room.Setting.LimitReaderEnter.CanEnter)
                {
                    foreach (string type in litype)
                    {
                        if (type == readerInfo.ReaderType)
                        {
                            Response.Write("<html><head><title>系统提示</title><script>alert('你的读者类型不能在该阅览室预约座位');top.Dialog.close();</script></head><body></body></html>");
                            Response.End();
                        }
                    }
                }
                else
                {
                    bool isintype = false;
                    foreach (string type in litype)
                    {
                        if (type == readerInfo.ReaderType)
                        {
                            isintype = true;
                            break;
                        }
                    }
                    if (!isintype)
                    {
                        Response.Write("<html><head><title>系统提示</title><script>alert('你的读者类型不能在该阅览室预约座位');top.Dialog.close();</script></head><body></body></html>");
                        Response.End();
                    }
                }
            }

            string isOpenRadio = "0";
            if (room.Setting.SeatBespeak.SpecifiedBespeak)//是否开启指定时间预约功能
            {
                isOpenRadio = "1";
            }
            ViewBag.isOpenRadio = isOpenRadio;
            ViewBag.roomOpenTime = roomOpenTime;
            ViewBag.lblEndDate = bespeakSureTimeSpan(room.Setting, "0", date);
            ViewBag.seatNo = seatNo;
            ViewBag.seatShortNo = seatShortNo;
            ViewBag.date = date;
            ViewBag.roomNo = roomNo;
            return View();
        }

        /// <summary>
        ///单选按钮切换回开馆预约的时候调用
        /// </summary>
        /// <param name="roomNo"></param>
        /// <param name="rblModelSelectedValue"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public string bespeakSureTimeSpan(string roomNo, string rblModelSelectedValue, string date)
        {
            SeatManage.Bll.T_SM_ReadingRoom bllReadingRoom = new SeatManage.Bll.T_SM_ReadingRoom();
            SeatManage.ClassModel.ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(roomNo);
            DateTime bespeakTime = Convert.ToDateTime(bllReadingRoom.GetRoomOpenTimeByDate(room.Setting, date).BeginTime);
            if (rblModelSelectedValue == "1")
            {
                if (room.Setting.SeatBespeak.SpecifiedTime)
                {
                    bespeakTime = room.Setting.SeatBespeak.SpecifiedTimeList[0];
                }
            }
            DateTime bespeakBeginTime = bespeakTime.AddMinutes(-double.Parse(room.Setting.SeatBespeak.ConfirmTime.BeginTime));
            DateTime bespeakEndTime = bespeakTime.AddMinutes(double.Parse(room.Setting.SeatBespeak.ConfirmTime.EndTime));
            return string.Format("{0}至{1}", bespeakBeginTime.ToShortTimeString(), bespeakEndTime.ToShortTimeString());
        }

        string bespeakSureTimeSpan(SeatManage.ClassModel.ReadingRoomSetting set, string rblModelSelectedValue, string date)
        {
            SeatManage.Bll.T_SM_ReadingRoom bllReadingRoom = new SeatManage.Bll.T_SM_ReadingRoom();
            DateTime bespeakTime = Convert.ToDateTime(bllReadingRoom.GetRoomOpenTimeByDate(set, date).BeginTime);
            if (rblModelSelectedValue == "1")
            {
                if (set.SeatBespeak.SpecifiedTime)
                {
                    bespeakTime = set.SeatBespeak.SpecifiedTimeList[0];
                }
            }
            DateTime bespeakBeginTime = bespeakTime.AddMinutes(-double.Parse(set.SeatBespeak.ConfirmTime.BeginTime));
            DateTime bespeakEndTime = bespeakTime.AddMinutes(double.Parse(set.SeatBespeak.ConfirmTime.EndTime));
            return string.Format("{0}至{1}", bespeakBeginTime.ToShortTimeString(), bespeakEndTime.ToShortTimeString());
        }

        /// <summary>
        /// 绑定图书馆下拉列表
        /// </summary>
        public string BindLibaray()
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
        /// <summary>
        /// 绑定预约阅览室列表
        /// </summary>
        /// <param name="libno"></param>
        /// <param name="SelectedDate"></param>
        /// <returns></returns>
        public string BindingGrid(string libno, string SelectedDate)
        {
            string dateStr = String.IsNullOrEmpty(SelectedDate) ? DateTime.Now.AddDays(1).ToShortDateString() : SelectedDate;
            string libnoStr = libno;
            if (string.IsNullOrEmpty(libno))
            {
                List<SeatManage.ClassModel.LibraryInfo> listLibrary = new List<SeatManage.ClassModel.LibraryInfo>();
                listLibrary = SeatManage.Bll.T_SM_Library.GetLibraryInfoList(null, null, null);
                libnoStr = listLibrary[0].No;
            }


            StringBuilder sb = new StringBuilder();
            DateTime date = DateTime.Parse(dateStr);
            DataTable dt = LogQueryHelper.BespeakRoomList(date, libnoStr);
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("{\"roomNum\": '" + r["roomNum"] + "',\"roomName\": \"" + r["roomName"] + "\",\"libraryName\": \"" + r["libraryName"] + "\",\"CanBespeakAmcount\": \"" + r["CanBespeakAmcount"] + "\",\"SurplusBespeskAmcount\": \"" + r["SurplusBespeskAmcount"] + "\"}");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append("}");

            return sb.ToString();
        }

        public ActionResult BespeakSeat()
        {
            string data = BindingGrid(string.Empty, string.Empty);
            ViewBag.InitData = data;
            ViewBag.InitDay = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            return View();
        }
        #endregion

        #region 预约当天
        /// <summary>
        /// 绑定预约阅览室列表(当天)
        /// </summary>
        /// <param name="libno"></param>
        /// <param name="SelectedDate"></param>
        /// <returns></returns>
        public string BindingNowDatGrid(string libno)
        {
            string libnoStr = libno;
            if (string.IsNullOrEmpty(libno))
            {
                List<SeatManage.ClassModel.LibraryInfo> listLibrary = new List<SeatManage.ClassModel.LibraryInfo>();
                listLibrary = SeatManage.Bll.T_SM_Library.GetLibraryInfoList(null, null, null);
                libnoStr = listLibrary[0].No;
            }
            StringBuilder sb = new StringBuilder();
            DataTable dt = LogQueryHelper.NowDayBespeakRoomInfo(libnoStr);
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("{\"roomNum\": '" + r["roomNum"] + "',\"roomName\": \"" + r["roomName"] + "\",\"libraryName\": \"" + r["libraryName"] + "\",\"SeatAmount\": \"" + r["SeatAmount"] + "\",\"SurplusBespeskAmcount\": \"" + r["SurplusBespeskAmcount"] + "\"}");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }


        public ActionResult BespeakNowDaySeat()
        {
            string data = BindingNowDatGrid(string.Empty);
            ViewBag.InitData = data;
            return View();
        }


        public JsonResult CheckingBespeakNowDay(string roomNo, string canBespeakAmountString)
        {
            JsonResult result = null;
            DateTime nowDate = SeatManage.Bll.ServiceDateTime.Now;
            ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(roomNo);
            string roomSet = room.Setting.ToString();
            if (string.IsNullOrEmpty(roomSet))
            {
                result = Json(new { status = "no", message = "该阅览室没有配置" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    SeatManage.ClassModel.ReadingRoomSetting set = new SeatManage.ClassModel.ReadingRoomSetting(roomSet);
                    int canBespeakAmount = int.Parse(canBespeakAmountString);

                    if (SeatManageWebV5.Code.NowReadingRoomState.ReadingRoomOpenState(set.RoomOpenSet, nowDate) == SeatManage.EnumType.ReadingRoomStatus.Close)
                    {
                        result = Json(new { status = "no", message = "阅览室没有开放" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (!set.SeatBespeak.Used || !set.SeatBespeak.NowDayBespeak)
                    {
                        result = Json(new { status = "no", message = "阅览室没有开放预约" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (canBespeakAmount <= 0)
                    {
                        result = Json(new { status = "no", message = "没有空余座位" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string roomNum = room.No;
                        string urlParametersString = SeatManage.SeatManageComm.AESAlgorithm.DESEncode(string.Format("roomNo={0}", roomNum));
                        result = Json(new { status = "yes", message = "预约座位", urlParameters = urlParametersString }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    result = Json(new { status = "no", message = string.Format("阅览室设置不正确：{0}", ex.Message) }, JsonRequestBehavior.AllowGet);
                }
            }
            return result;
        }


        public ActionResult BespeakNowDaySeatLayout()
        {
            string roomNum = "";
            if (!OpVerifiction())
            {
                Response.Write("<html><head><title>系统安全提示</title><script>alert('请使用正常方式访问网站');location.href='/seat/Login'</script></head><body></body></html>");
                Response.End();
            }
            else if (string.IsNullOrEmpty(Request.QueryString["Param"]))
            {
                Response.Write("<html><head><title>系统安全提示</title><script>alert('请使用正常方式访问网站');location.href='/seat/Login'</script></head><body></body></html>");
                Response.End();
            }
            else
            {
                BespeakSubmitWindowParamModel par = new BespeakSubmitWindowParamModel(Request.QueryString["Param"]);
                roomNum = par.RoomNo;
            }
            ViewBag.RoomNo = roomNum;
            return View();
        }

        public string NowBespeakSeatLayoutHTML(string roomNum, string divTransparentTop, string divTransparentLeft)
        {
            return new SeatLayoutTools().DrawNowBespeakSeatLayoutHTML(roomNum, divTransparentTop, divTransparentLeft);
        }

        /// <summary>
        /// 打开当天预约座位确认窗口
        /// </summary>
        /// <returns></returns>
        public ActionResult BespeakNowDayHandle()
        {
            if (Request.ServerVariables["HTTP_REFERER"] != null)
            {
                string url = Request.ServerVariables["HTTP_REFERER"].Trim();
                string pageName = SeatManage.SeatManageComm.SeatComm.GetPageName(url);
                if (pageName.ToUpper() != "INDEX")
                {
                    WriteLogs("阅览室布局页面");
                    Response.Write("<html><head><title>系统安全提示</title><script>alert('请通过正确方式访问网站');location.href='/Login'</script></head><body></body></html>");
                    Response.End();
                }
            }
            else
            {
                WriteLogs("阅览室布局页面");
                WriteLogs(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                Response.Write("<html><head><title>系统安全提示</title><script>alert('请通过正确方式访问网站');location.href='/Login'</script></head><body></body></html>");
                Response.End();
            }

            string parameters = Request.QueryString["parameters"];
            SeatManageWebV5.Code.BespeakSubmitWindowParamModel bespeakSubmitModel = new BespeakSubmitWindowParamModel(parameters);
            string seatNo = bespeakSubmitModel.SeatNo;
            string seatShortNo = bespeakSubmitModel.ShortSeatNo;
            DateTime date = SeatManage.Bll.ServiceDateTime.Now;
            string roomNo = bespeakSubmitModel.RoomNo;

            SeatManage.ClassModel.ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(roomNo);
            if (!IsCanBespeakNowDay(roomNo))
            {
                // btnBespeak.Enabled = false;
                // return;
                Response.Write("<html><head><title>系统提示</title><script>alert('阅览室没有开放预约');top.Dialog.close();</script></head><body></body></html>");
                Response.End();
            }

            //lblbeginDate.Text = date.ToShortDateString();
            //this.lblSeatNo.Text = seatShortNo;
            //this.lblRoomName.Text = room.Name;
            //判断自己是否已经预约座位
            List<SeatManage.EnumType.BookingStatus> bespeakStatus = new List<SeatManage.EnumType.BookingStatus>();
            bespeakStatus.Add(SeatManage.EnumType.BookingStatus.Waiting);
            List<SeatManage.ClassModel.BespeakLogInfo> readerBespeaklist = SeatManage.Bll.T_SM_SeatBespeak.GetBespeakLogInfoByCardNo(this.LoginId, date);//.GetBespeakList(this.LoginId, null, date, 0, bespeakStatus);
            if (readerBespeaklist.Count > 0)
            {
                Response.Write("<html><head><title>系统提示</title><script>alert('您今天已有预约的座位，请先取消原来的预约。');top.Dialog.close();</script></head><body></body></html>");
                Response.End();
            }
            //判断可预约次数是否超过
            readerBespeaklist = SeatManage.Bll.T_SM_SeatBespeak.GetBespeakList(this.LoginId, null, date, 0, new List<SeatManage.EnumType.BookingStatus> { SeatManage.EnumType.BookingStatus.Confinmed });//.GetBespeakList(this.LoginId, null, date, 0, bespeakStatus);
            if (readerBespeaklist.Count >= room.Setting.SeatBespeak.BespeakSeatCount)
            {
                Response.Write("<html><head><title>系统提示</title><script>alert('您一天只能预约" + room.Setting.SeatBespeak.BespeakSeatCount + "次座位。');top.Dialog.close();</script></head><body></body></html>");
                Response.End();
            }
            //判断读者是否有座位
            if (!room.Setting.SeatBespeak.BespeatWithOnSeat)
            {
                SeatManage.ClassModel.EnterOutLogInfo eol = SeatManage.Bll.T_SM_EnterOutLog.GetEnterOutLogInfoByCardNo(this.LoginId);
                if (eol != null && eol.EnterOutState != SeatManage.EnumType.EnterOutLogType.Leave)
                {
                    Response.Write("<html><head><title>系统提示</title><script>alert('您已有座位，不能再预约座位。');top.Dialog.close();</script></head><body></body></html>");
                    Response.End();
                }
            }
            //判断操作次数
            if (SeatManage.Bll.EnterOutOperate.CheckReaderChooseSeatTimesByReadingRoom(this.LoginId, room.Setting.PosTimes, room.No))
            {
                Response.Write("<html><head><title>系统提示</title><script>alert('您的选座/预约操作过于频繁，请稍后重试');top.Dialog.close();</script></head><body></body></html>");
                Response.End();
            }
            //判断座位是否被别人使用
            SeatManage.ClassModel.Seat seat = SeatManage.Bll.T_SM_Seat.GetSeatInfoBySeatNo(seatNo);
            if (seat.SeatUsedState != SeatManage.EnumType.EnterOutLogType.Leave)
            {
                Response.Write("<html><head><title>系统提示</title><script>alert('座位已经被别人选择，请预约其他座位');top.Dialog.close();</script></head><body></body></html>");
                Response.End();
            }
            //判断座位是否被预约
            List<SeatManage.ClassModel.BespeakLogInfo> list = SeatManage.Bll.T_SM_SeatBespeak.GetBespeakLogInfoBySeatNo(seatNo, date);
            //  seatKeepTime.Value = room.Setting.SeatBespeak.SeatKeepTime.ToString();
            string lblEndDate = string.Format("{0}至{1}", date.ToShortTimeString(), date.AddMinutes(room.Setting.SeatBespeak.SeatKeepTime).ToShortTimeString());
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].BsepeakState == SeatManage.EnumType.BookingStatus.Waiting)
                {
                    Response.Write("<html><head><title>系统提示</title><script>alert('座位已经被别人预约，请预约其他座位');top.Dialog.close();</script></head><body></body></html>");
                    Response.End();
                }
            }
            //判断是否已加入黑名单
            List<SeatManage.ClassModel.BlackListInfo> blacklistInfoByCardNo = SeatManage.Bll.T_SM_Blacklist.GetBlackListInfo(this.LoginId);
            SeatManage.ClassModel.RegulationRulesSetting rulesSet = SeatManage.Bll.T_SM_SystemSet.GetRegulationRulesSetting();
            if (room.Setting.UsedBlacklistLimit && blacklistInfoByCardNo.Count > 0)
            {
                if (room.Setting.BlackListSetting.Used)
                {
                    bool isblack = false;
                    foreach (SeatManage.ClassModel.BlackListInfo blinfo in blacklistInfoByCardNo)
                    {
                        if (blinfo.ReadingRoomID == room.No)
                        {
                            isblack = true;
                            break;
                        }
                    }
                    if (isblack)
                    {
                        Response.Write("<html><head><title>系统提示</title><script>alert('你已进入黑名单，不能在该阅览室预约座位');top.Dialog.close();</script></head><body></body></html>");
                        Response.End();
                    }
                }
                else
                {
                    Response.Write("<html><head><title>系统提示</title><script>alert('你已进入黑名单，不能在该阅览室预约座位');top.Dialog.close();</script></head><body></body></html>");
                    Response.End();
                }
            }
            if (room.Setting.LimitReaderEnter.Used)
            {
                SeatManage.ClassModel.ReaderInfo readerInfo = SeatManage.Bll.EnterOutOperate.GetReaderInfo(this.LoginId);
                string[] litype = room.Setting.LimitReaderEnter.ReaderTypes.Split(';');
                if (!room.Setting.LimitReaderEnter.CanEnter)
                {
                    foreach (string type in litype)
                    {
                        if (type == readerInfo.ReaderType)
                        {
                            Response.Write("<html><head><title>系统提示</title><script>alert('你的读者类型不能在该阅览室预约座位');top.Dialog.close();</script></head><body></body></html>");
                            Response.End();
                        }
                    }
                }
                else
                {
                    bool isintype = false;
                    foreach (string type in litype)
                    {
                        if (type == readerInfo.ReaderType)
                        {
                            isintype = true;
                            break;
                        }
                    }
                    if (!isintype)
                    {
                        Response.Write("<html><head><title>系统提示</title><script>alert('你的读者类型不能在该阅览室预约座位');top.Dialog.close();</script></head><body></body></html>");
                        Response.End();
                    }
                }
            }

            string isOpenRadio = "0";
            if (room.Setting.SeatBespeak.SpecifiedBespeak)//是否开启指定时间预约功能
            {
                isOpenRadio = "1";
            }
            ViewBag.isOpenRadio = isOpenRadio;
            ViewBag.seatNo = seatNo;
            ViewBag.seatShortNo = seatShortNo;
            ViewBag.date = SeatManage.Bll.ServiceDateTime.Now;//date.ToShortDateString();
            ViewBag.roomNo = roomNo;
            ViewBag.lblEndDate = string.Format("{0}至{1}", date.ToShortTimeString(), date.AddMinutes(room.Setting.SeatBespeak.SeatKeepTime).ToShortTimeString());
            return View();
        }

        /// <summary>
        /// 判断座位是否符合预约条件
        /// </summary>
        /// <returns></returns>
        protected bool IsCanBespeakNowDay(string roomNo)
        {
            bool result = true;
            SeatManage.ClassModel.ReadingRoomSetting set = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(roomNo).Setting;
            if (!set.SeatBespeak.Used || !set.SeatBespeak.NowDayBespeak)
            {
                Response.Write("<html><head><title>系统提示</title><script>alert('阅览室没有开放预约');top.Dialog.close();</script></head><body></body></html>");
                Response.End();
            }
            return result;
        }


        public string RbtnBespeakModelChange(string roomNo, string rblModelSelectedValue)
        {
            SeatManage.ClassModel.ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(roomNo);
            string lblEndDate = "";
            StringBuilder sb = new StringBuilder();
            if (rblModelSelectedValue == "0")
            {
                lblEndDate = SelectTimeChangeNowDay(roomNo, DateTime.Now.ToString());//string.Format("{0}至{1}", bespeakTime.ToShortTimeString(), bespeakTime.AddMinutes(room.Setting.SeatBespeak.SeatKeepTime).ToShortTimeString());
            }
            else if (rblModelSelectedValue == "1")
            {

                //DateTime date = DateTime.Parse(dateString);
                DateTime minTime = DateTime.Now.AddMinutes(10 - DateTime.Now.Minute % 10);
                sb.Append("<select onchange=\"selChange()\" prompt=\"请选择\" id=\"timeSelect\">");
                while (true)
                {
                    minTime = minTime.AddMinutes(10);
                    if (minTime.Date > DateTime.Now.Date)
                    {
                        break;
                    }
                    if (SeatManageWebV5.Code.NowReadingRoomState.ReadingRoomOpenState(room.Setting.RoomOpenSet, minTime) == SeatManage.EnumType.ReadingRoomStatus.Close)
                    {
                        continue;
                    }
                    sb.Append("<option value=\"" + minTime.ToShortTimeString() + "\">" + minTime.ToShortTimeString() + "</option>");
                }
                sb.Append("</select>");
            }
            ViewBag.lblEndDate = lblEndDate;
            return sb.ToString();
        }

        public string SelectTimeChangeNowDay(string roomNo, string selValue)
        {
            SeatManage.ClassModel.ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(roomNo);
            DateTime bespeakTime = DateTime.Parse(selValue);
            DateTime bespeakBeginTime = bespeakTime.AddMinutes(-double.Parse(room.Setting.SeatBespeak.ConfirmTime.BeginTime));
            DateTime bespeakEndTime = bespeakTime.AddMinutes(double.Parse(room.Setting.SeatBespeak.ConfirmTime.EndTime));
            string lblEndDate = string.Format("{0}至{1}", bespeakBeginTime.ToShortTimeString(), bespeakEndTime.ToShortTimeString());
            return lblEndDate;
        }


        public JsonResult btnBespeakSubmitNowDay(string roomNo, string seatNo, string dateString, string rblModelSelectedValue, string DropDownList_FreeTimeSelectedText)
        {
            JsonResult returnResult = null;
            SeatManage.ClassModel.BespeakLogInfo bespeakModel = new SeatManage.ClassModel.BespeakLogInfo();
            bespeakModel.BsepeakState = SeatManage.EnumType.BookingStatus.Waiting;
            bespeakModel.BsepeakTime = DateTime.Parse(dateString);
            bespeakModel.CardNo = this.LoginId;
            bespeakModel.ReadingRoomNo = roomNo.Trim();
            if (rblModelSelectedValue == "1")
            {
                bespeakModel.BsepeakTime = DateTime.Parse(string.Format("{0} {1}", DateTime.Parse(dateString).ToShortDateString(), DropDownList_FreeTimeSelectedText));
            }

            SeatManage.ClassModel.ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(bespeakModel.ReadingRoomNo);
            if (room.Setting.ReadingRoomOpenState(DateTime.Now) == ReadingRoomStatus.Close)
            {
                returnResult = Json(new { status = "no", message = "当前时间阅览室未开放" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                bespeakModel.Remark = string.Format("读者通过Web页面预约当天座位");
                bespeakModel.SeatNo = seatNo;
                bespeakModel.SubmitTime = DateTime.Parse(dateString);
                if (bespeakModel.BsepeakTime < bespeakModel.SubmitTime)
                {
                    returnResult = Json(new { status = "no", message = "对不起，预约的时间不正确，请刷新页面重新选择" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    try
                    {
                        List<SeatManage.ClassModel.BespeakLogInfo> seatbespeakLog = SeatManage.Bll.T_SM_SeatBespeak.GetBespeakLogInfoBySeatNo(seatNo, DateTime.Parse(dateString));
                        Seat seatInfo = SeatManage.Bll.T_SM_Seat.GetSeatInfoBySeatNo(seatNo);
                        SeatManage.EnumType.HandleResult result = SeatManage.Bll.T_SM_SeatBespeak.AddBespeakLogInfo(bespeakModel);
                        if (bespeakModel.BsepeakTime == DateTime.Now.Date && seatInfo.SeatUsedState != EnterOutLogType.Leave) //如果启用预约，判断选择的日期是否为当天的日期
                        {
                            returnResult = Json(new { status = "no", message = "对不起，你所预约的座位正在使用。" }, JsonRequestBehavior.AllowGet);
                        }

                        else if (seatbespeakLog.Count > 0)
                        {
                            returnResult = Json(new { status = "no", message = "对不起，该座位已经被别人预约。" }, JsonRequestBehavior.AllowGet);
                        }

                        else if (result == SeatManage.EnumType.HandleResult.Successed)
                        {
                            returnResult = Json(new { status = "yes", message = "座位预约成功，请在规定的时间内刷卡确认。" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            returnResult = Json(new { status = "no", message = "对不起，该座位已经被别人预约。" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        returnResult = Json(new { status = "no", message = string.Format("执行预约操作遇到错误：{0}", ex.Message) }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return returnResult;
        }


        #endregion

        public ActionResult BespeakProcess()
        {
            return View();
        }
    }
}