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

    public class SeatMonitorController : BaseController
    {
        // GET: SeatMonitor
        public ActionResult Index()
        {
           
            return View();
        }

        public void BindSingleSeat(string seatNo, string seatShortNo, string used)
        {
            if (!string.IsNullOrEmpty(seatNo)) //如果座位号不为空，正常打开
            {
                //lblSeatNo.Text = seatShortNo;
                ViewBag.SeatNo = seatShortNo;

                if (used == "0") //没人
                {
                    //lblCardNo.Text = "无";
                    ViewBag.CardNo = "无";

                    //lblName.Text = "无";
                    ViewBag.Name = "无";

                    //lblSeatStatus.Text = "空闲";
                    ViewBag.SeatStatus = "空闲";
                   
                    // lblTimeLength.Text = "";
                    ViewBag.TimeLength = "";

                   // txtSeat.Text = seatShortNo;
                    ViewBag.Seat=seatShortNo;

                    //btnAddBlackList.Enabled = false;
                    ViewBag.AddBlackListEnabled = "false";

                    //btnShortLeave.Enabled = false;
                    ViewBag.ShortLeaveEnabled = "false";

                  //  btnShortLeave.Text = "暂离";
                    ViewBag.btnShortLeave = "暂离";

                    //btnShortLeave.ConfirmText = "是否确定把该读者设置为暂离？";
                    ViewBag.btnShortLeaveConfirmText = "是否确定把该读者设置为暂离？";

                   // btnLeave.Enabled = false;
                    ViewBag.btnLeave = "false";

                    //btnAllotSeat.Enabled = true;
                    ViewBag.btnAllotSeatEnabled = "true";
                }
                else if (used == "2") 
                {
                    List<SeatManage.ClassModel.BespeakLogInfo> list = SeatManage.Bll.T_SM_SeatBespeak.GetBespeakLogInfoBySeatNo(seatNo, SeatManage.Bll.ServiceDateTime.Now);
                    if (list == null)
                    {
                        //FineUI.Alert.ShowInTop("没有获取到相关的座位信息", "错误");
                        Response.Write("<html><head><title>系统提示</title><script>alert('没有获取到相关的座位信息');</script></head><body></body></html>");
                        Response.End();
                    }
                    else
                    {
                        //txtSeat.Text = seatShortNo;
                        ViewBag.SeatNo = seatShortNo;

                        //lblCardNo.Text = list[0].CardNo;
                        ViewBag.CardNo = list[0].CardNo;

                        //lblName.Text = list[0].ReaderName;
                        ViewBag.Name = list[0].ReaderName;

                        //lblSeatStatus.Text = "已被预约";
                        ViewBag.SeatStatus = "已被预约";

                      //  lblTimeLength.Text = string.Format("{0:MM月dd日 HH:mm:ss}", list[0].BsepeakTime);
                        ViewBag.TimeLength = string.Format("{0:MM月dd日 HH:mm:ss}", list[0].BsepeakTime);

                        //btnAddBlackList.Enabled = false;
                        ViewBag.AddBlackListEnabled = "false";

                        //btnShortLeave.Enabled = false;
                        ViewBag.ShortLeaveEnabled = "false";

                      //  btnShortLeave.Text = "暂离";
                        ViewBag.btnShortLeave = "暂离";

                       // btnShortLeave.ConfirmText = "是否确定把该读者设置为暂离？";
                        ViewBag.btnShortLeaveConfirmText = "是否确定把该读者设置为暂离？";

                       // btnLeave.Enabled = false;
                        ViewBag.btnLeave = "false";

                       // btnAllotSeat.Enabled = false;
                        ViewBag.btnAllotSeatEnabled = "false";
                    }
                }
                else if (used == "3") //座位停用
                {
                    //  FineUI.Alert.ShowInTop("此座位已暂停使用", "提示");
                    Response.Write("<html><head><title>系统提示</title><script>alert('此座位已暂停使用');</script></head><body></body></html>");
                    Response.End();

                  //  btnAddBlackList.Enabled = false;
                    ViewBag.AddBlackListEnabled = "false";

                  //  btnShortLeave.Enabled = false;
                    ViewBag.ShortLeaveEnabled = "false";

                 //   btnShortLeave.ConfirmText = "此座位已暂停使用，请重新选择";
                    ViewBag.btnShortLeaveConfirmText = "此座位已暂停使用，请重新选择";

                  //  btnLeave.Enabled = false;
                    ViewBag.btnLeave = "false";

                  //  btnAllotSeat.Enabled = false;
                    ViewBag.btnAllotSeatEnabled = "false";

                }
                else
                {
                    SeatManage.ClassModel.Seat seat = SeatManage.Bll.T_SM_Seat.GetSeatInfoBySeatNo(seatNo);
                    if (seat == null)
                    {
                        Response.Write("<html><head><title>系统提示</title><script>alert('没有获取到相关的座位信息');</script></head><body></body></html>");
                        Response.End();
                        //  FineUI.Alert.ShowInTop("没有获取到相关的座位信息", "错误");
                    }
                    else
                    {
                        if (seat.SeatUsedState == SeatManage.EnumType.EnterOutLogType.Leave)
                        {
                            //lblCardNo.Text = "无";
                            ViewBag.CardNo = "无";

                           // lblName.Text = "无";
                            ViewBag.Name = "无";

                           // lblSeatStatus.Text = "空闲";
                            ViewBag.SeatStatus = "空闲";

                         //   lblTimeLength.Text = "";
                            ViewBag.TimeLength = "";

                          //  txtSeat.Text = seat.ShortSeatNo;
                            ViewBag.Seat = seat.ShortSeatNo;

                           // btnAddBlackList.Enabled = false;
                            ViewBag.AddBlackListEnabled = "false";

                           // btnShortLeave.Enabled = false;
                            ViewBag.ShortLeaveEnabled = "false";

                           // btnShortLeave.Text = "暂离";
                            ViewBag.btnShortLeave = "暂离";

                         //   btnShortLeave.ConfirmText = "是否确定把该读者设置为暂离？";
                            ViewBag.btnShortLeaveConfirmText = "是否确定把该读者设置为暂离";

                          //  btnLeave.Enabled = false;
                            ViewBag.btnLeave = "false";

                          //  btnAllotSeat.Enabled = true;
                            ViewBag.btnAllotSeatEnabled = "false";
                        }
                        else if (seat.SeatUsedState == SeatManage.EnumType.EnterOutLogType.ShortLeave)
                        {
                          //  txtCardNo.Text = seat.UserCardNo;
                            ViewBag.CardNo = seat.UserCardNo;

                         //   txtCardNo1.Text = seat.UserCardNo;
                            ViewBag.txtCardNo1 = seat.UserCardNo;

                            //txtReaderName.Text = seat.UserName;
                            ViewBag.ReaderNameText = seat.UserName;

                            //txtSeat.Text = seat.ShortSeatNo;
                            ViewBag.Seat = seat.ShortSeatNo;

                         //   lblCardNo.Text = seat.UserCardNo;
                            ViewBag.CardNo = seat.UserCardNo;

                           // lblName.Text = seat.UserName;
                            ViewBag.Name = seat.UserName;

                         //   lblSeatStatus.Text = SeatManage.SeatManageComm.SeatComm.ConvertReaderState(seat.SeatUsedState);
                            ViewBag.SeatStatus = SeatManage.SeatManageComm.SeatComm.ConvertReaderState(seat.SeatUsedState);

                            //lblTimeLength.Text = string.Format("{0:MM月dd日 HH:mm:ss}", seat.BeginUsedTime);
                            ViewBag.TimeLength = string.Format("{0:MM月dd日 HH:mm:ss}", seat.BeginUsedTime);

                           // btnAddBlackList.Enabled = true;
                            ViewBag.AddBlackListEnabled = "true";

                           // btnShortLeave.Enabled = true;
                            ViewBag.ShortLeaveEnabled = "true";

                           // btnShortLeave.Text = "取消暂离";
                            ViewBag.btnShortLeave = "取消暂离";

                            //btnShortLeave.ConfirmText = "是否取消此读者的暂离状态，并还原为在座状态？";
                            ViewBag.btnShortLeaveConfirmText = "是否取消此读者的暂离状态，并还原为在座状态？";

                            //btnLeave.Enabled = true;
                            ViewBag.btnLeave = "true";

                          //  btnAllotSeat.Enabled = false;
                            ViewBag.btnAllotSeatEnabled = "false";

                        }
                        else
                        {
                          //  txtCardNo.Text = seat.UserCardNo;
                            ViewBag.CardNo = seat.UserCardNo;

                          //  txtCardNo1.Text = seat.UserCardNo;
                            ViewBag.txtCardNo1 = seat.UserCardNo;

                          //  txtReaderName.Text = seat.UserName;
                            ViewBag.ReaderNameText = seat.UserName;

                         //   txtSeat.Text = seat.ShortSeatNo;
                            ViewBag.Seat = seat.ShortSeatNo;

                          //  lblCardNo.Text = seat.UserCardNo;
                            ViewBag.CardNo = seat.UserCardNo;

                         //   lblName.Text = seat.UserName;
                            ViewBag.Name = seat.UserName;

                        //    lblSeatStatus.Text = SeatManage.SeatManageComm.SeatComm.ConvertReaderState(seat.SeatUsedState);
                            ViewBag.SeatStatus = SeatManage.SeatManageComm.SeatComm.ConvertReaderState(seat.SeatUsedState);

                          //  lblTimeLength.Text = string.Format("{0:MM月dd日 HH:mm:ss}", seat.BeginUsedTime);
                            ViewBag.TimeLength = string.Format("{0:MM月dd日 HH:mm:ss}", seat.BeginUsedTime);

                         //   btnAddBlackList.Enabled = true;
                            ViewBag.AddBlackListEnabled = "true";

                          //  btnShortLeave.Enabled = true;
                            ViewBag.ShortLeaveEnabled = "true";

                           // btnShortLeave.Text = "暂离";
                            ViewBag.btnShortLeave = "暂离";

                          //  btnShortLeave.ConfirmText = "是否确定把该读者设置为暂离？";
                            ViewBag.btnShortLeaveConfirmText = "是否确定把该读者设置为暂离？";

                           // btnLeave.Enabled = true;
                            ViewBag.btnLeave = "true";

                           // btnAllotSeat.Enabled = false;
                            ViewBag.btnAllotSeatEnabled = "false";
                        }
                    }
                }
            }
            else
            {
                Response.Write("<html><head><title>系统提示</title><script>alert('座位号不能为空');</script></head><body></body></html>");
                Response.End();
                //  FineUI.Alert.ShowInTop("座位号不能为空", "错误");
            }
        }


        public JsonResult ShortLeave(string seatNo, string seatShortNo, string used)
        {
            return null;
        }

        public JsonResult Leave(string seatNo, string seatShortNo, string used)
        {
            return null;
        }

        public JsonResult SureAddBlacklist(string seatNo, string seatShortNo, string used)
        {
            return null;
        }

        public JsonResult SureAllotSeat(string seatNo, string seatShortNo, string used)
        {
            JsonResult ret = null;

            SeatManage.ClassModel.Seat seat = SeatManage.Bll.T_SM_Seat.GetSeatInfoBySeatNo(seatNo);
            SeatManage.ClassModel.ReadingRoomInfo roomInfo = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(seat.ReadingRoomNum);
            if (seat == null)
            {
                ret = Json(new { status = "no", message = "座位号错误，没有找到座位的相关信息" }, JsonRequestBehavior.AllowGet);
                //FineUI.Alert.ShowInTop("座位号错误，没有找到座位的相关信息");
                //return;
            }
            //判断当前座位上是否有读者在座。
            SeatManage.ClassModel.EnterOutLogInfo enterOutLogByseatNo = SeatManage.Bll.T_SM_EnterOutLog.GetUsingEnterOutLogBySeatNo(seatNo);
            if (enterOutLogByseatNo != null && enterOutLogByseatNo.EnterOutState != SeatManage.EnumType.EnterOutLogType.Leave)
            {
                ret = Json(new { status = "no", message = "座位已经被其他读者选择" }, JsonRequestBehavior.AllowGet);

                //FineUI.Alert.ShowInTop("座位已经被其他读者选择");
                //return;
            }
            //判断读者是否有座位
            string strCardNo = "reader";// txtCardNo1.Text;
            List<SeatManage.ClassModel.BlackListInfo> blacklistInfoByCardNo = SeatManage.Bll.T_SM_Blacklist.GetBlackListInfo(strCardNo);
            SeatManage.ClassModel.RegulationRulesSetting rulesSet = SeatManage.Bll.T_SM_SystemSet.GetRegulationRulesSetting();
            if (roomInfo.Setting.UsedBlacklistLimit && blacklistInfoByCardNo.Count > 0)
            {
                if (roomInfo.Setting.BlackListSetting.Used)
                {
                    bool isblack = false;
                    foreach (SeatManage.ClassModel.BlackListInfo blinfo in blacklistInfoByCardNo)
                    {
                        if (blinfo.ReadingRoomID == roomInfo.No)
                        {
                            isblack = true;
                            break;
                        }
                    }
                    if (isblack)
                    {
                        ret = Json(new { status = "no", message = "该读者已进入黑名单，不能在该阅览室为其分配座位" }, JsonRequestBehavior.AllowGet);

                        //FineUI.Alert.ShowInTop("该读者已进入黑名单，不能在该阅览室为其分配座位");
                        //return;
                    }
                }
                else
                {
                    ret = Json(new { status = "no", message = "该读者已进入黑名单，不能在该阅览室为其分配座位" }, JsonRequestBehavior.AllowGet);


                    //FineUI.Alert.ShowInTop("该读者已进入黑名单，不能在该阅览室为其分配座位");
                    //return;
                }
            }
            SeatManage.ClassModel.EnterOutLogInfo enterOutLogByCardNo = SeatManage.Bll.T_SM_EnterOutLog.GetEnterOutLogInfoByCardNo(strCardNo);
            if (enterOutLogByCardNo != null && enterOutLogByCardNo.EnterOutState != SeatManage.EnumType.EnterOutLogType.Leave)
            {
                ret = Json(new { status = "no", message = string.Format("读者已经在{0}，{1}号座位就做", enterOutLogByCardNo.ReadingRoomName, enterOutLogByCardNo.ShortSeatNo) }, JsonRequestBehavior.AllowGet);


                //FineUI.Alert.ShowInTop(string.Format("读者已经在{0}，{1}号座位就做", enterOutLogByCardNo.ReadingRoomName, enterOutLogByCardNo.ShortSeatNo));
                //return;
            }

            SeatManage.ClassModel.EnterOutLogInfo enterOutLogModel = new SeatManage.ClassModel.EnterOutLogInfo();
            enterOutLogModel.CardNo = strCardNo;
            enterOutLogModel.EnterOutLogNo = SeatManage.SeatManageComm.SeatComm.RndNum();
            enterOutLogModel.EnterOutState = SeatManage.EnumType.EnterOutLogType.SelectSeat;
            enterOutLogModel.EnterOutTime = SeatManage.Bll.ServiceDateTime.Now;
            enterOutLogModel.EnterOutType = SeatManage.EnumType.LogStatus.Valid;
            enterOutLogModel.Flag = SeatManage.EnumType.Operation.Admin;
            enterOutLogModel.ReadingRoomNo = seat.ReadingRoomNum;
            enterOutLogModel.Remark = string.Format("在后台管理网站被管理员{0}，分配{1}，{2}座位", this.LoginId, roomInfo.Name, seat.SeatNo.Substring(seat.SeatNo.Length - roomInfo.Setting.SeatNumAmount));
            enterOutLogModel.SeatNo = seatNo;
            int newId = -1;
            SeatManage.EnumType.HandleResult result = SeatManage.Bll.EnterOutOperate.AddEnterOutLog(enterOutLogModel, ref newId);
            if (result == SeatManage.EnumType.HandleResult.Successed)
            {

                ret = Json(new { status = "no", message = "分配座位成功" }, JsonRequestBehavior.AllowGet);

                //FineUI.Alert.ShowInTop("分配座位成功", "成功");
                //PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            }
            else
            {
                ret = Json(new { status = "no", message = "分配座位失败" }, JsonRequestBehavior.AllowGet);

              //  FineUI.Alert.ShowInTop("分配座位失败", "失败");
            }

            return ret;
        }


        public ActionResult SeatHandle(string seatNo,string seatShortNo,string used)
        {
            BindSingleSeat(seatNo, seatShortNo, used);

            ViewBag.used = used;

            return View();
        }

        public string DrowSeatLayoutHtml(string roomNum, string divTransparentTop, string divTransparentLeft)
        {
            string html = "";
            Code.SeatLayoutTools tool = new Code.SeatLayoutTools();
            html = tool.drowSeatLayoutHtml(roomNum, divTransparentTop, divTransparentLeft);
            return html;
        }

        public ActionResult SeatGraph(string roomId)
        {
            ViewBag.roomId = roomId;
            return View();
        }

        private string MonitorGraphModeDataBind()
        {
            string result = "";
            DataTable dt = LogQueryHelper.GetMonitorGraphReadingRoomList(this.LoginId);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (DataRow item in dt.Rows)
            {
                sb.Append("{\"roomNum\": " + item["roomNum"] + ",\"roomName\": \"" + item["roomName"] + "\",\"libraryName\": \"" + item["libraryName"] + "\",\"seatCountAll\": \"" + item["seatCountAll"] + "\",\"seatCountUsed\": \"" + item["seatCountUsed"] + "\",\"seatCountShortLeave\": \"" + item["seatCountShortLeave"] + "\"}");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append("}");
            string data = sb.ToString();
            ViewBag.Data = data;
            return result;
        }

        public ActionResult MonitorGraphMode()
        {
            MonitorGraphModeDataBind();
            return View();
        }
        public ActionResult DeviceStatusInfo()
        {
            return View();
        }
    }
}