using Dos.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiXinMsgService;
using WeiXinMsgService.Model;

namespace WeChatMsgPushWeb
{
    /// <summary>
    /// Summary description for MsgHandler
    /// </summary>
    public class MsgHandler : IHttpHandler
    {

        private string signature = "";//微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。 
        private string timestamp = "";//时间戳 
        private string nonce = "";//随机数 
        private string echostr = "";//随机字符串 
        private HttpContext context = null;

        CommonTools tool = new CommonTools();

        public void ProcessRequest(HttpContext context)
        {
            this.context = context;
            //context.Response.ContentType = "text/plain";
            //string contenttype = "application/x-www-form-urlencoded";
            context.Response.ContentType = "application/x-www-form-urlencoded";
            try
            {
                if (this.context.Request.HttpMethod == "POST")
                {
                    /*
                    预约成功消息
                    SchoolNum=20180605&StudentNo=20152102166&MsgType=UserOperation&Room=二楼C区（普通阅览区）&SeatNo=033&AddTime=2018-07-15 16:57:56&EndTime=&Days=VRType=&Msg=您在移动客户端预约2018/7/22 8:00:00 在二楼C区（普通阅览区） 033号座位，请在7:40至8:40之间到图书馆刷卡确认。
                    终端机选座成功消息
                    SchoolNum=20180605&StudentNo=20155103373&MsgType=UserOperation&Room=二楼E区（普通阅览区）&SeatNo=079&AddTime=2018-07-15 17:01:37&EndTime=&Days=VRType=&Msg=在终端2018060503手动选择，二楼E区（普通阅览区），079号座位
                    终端机签到消息
                    SchoolNum=20180605&StudentNo=20155104027&MsgType=UserOperation&Room=二楼C区（普通阅览区）&SeatNo=027&AddTime=2018-07-16 07:41:51&EndTime=&Days=VRType=&Msg=在终端2018060506刷卡，入座预约的二楼C区（普通阅览区） 027号座位
                    座位释放通知
                    SchoolNum=20180605&StudentNo=20155103137&MsgType=UserOperation&Room=二楼D区（电子阅览区）&SeatNo=028&AddTime=2018-07-15 17:02:09&EndTime=&Days=VRType=&Msg=在终端2018060503刷卡释放二楼D区（电子阅览区） 028号座位
                    取消预约通知
                    SchoolNum=20180605&StudentNo=18186470276&
                    MsgType=UserOperation&Room=二楼E区（普通阅览区）&SeatNo=107&
                    AddTime=2018-07-15 21:33:21&EndTime=&Days=VRType=&Msg=您在移动终端上，取消二楼E区（普通阅览区） 107号座位，在2018/7/16 8:00:00的预约。 
                    */
                    string parm = SeatManage.SeatManageComm.AESAlgorithm.AESDecrypt(context.Request.Params["msg"].ToString(), "SeatManage_WeiCharCode");
                    if (parm.IndexOf("UserOperation", StringComparison.Ordinal) > 0) //UserOperation消息
                    {
                        AcceptUserOperationModel msgModel = new AcceptUserOperationModel();
                        string[] parms = parm.Split('&');

                        msgModel.SchoolNum = parms[0].Split('=')[1];
                        msgModel.StudentNo = parms[1].Split('=')[1];
                        msgModel.MsgType = parms[2].Split('=')[1];
                        msgModel.Room = parms[3].Split('=')[1];
                        msgModel.SeatNo = parms[4].Split('=')[1];
                        msgModel.AddTime = parms[5].Split('=')[1];
                        msgModel.Msg = parms[8].Split('=')[1];
                        tb_User u = GetOpenIDByCardNo(msgModel.StudentNo);
                        TemplateModel m = new TemplateModel(u.NickName+"您的座位状态发生改变", msgModel.Room, msgModel.SeatNo, msgModel.AddTime, msgModel.Msg);
                        Push(u.OpenId, m);
                    }
                }
                else if (this.context.Request.HttpMethod == "GET")//如果是Get请求，则是接入验证，返回随机字符串。
                {
                    signature = context.Request.Params["signature"];
                    timestamp = context.Request.Params["timestamp"];
                    nonce = context.Request.Params["nonce"];
                    echostr = context.Request.Params["echostr"];

                    if (!tool.CheckSignature(signature, timestamp, nonce))//验证请求是否微信发过来的。
                    {//不是则结束响应
                        SeatManage.SeatManageComm.WriteLog.Write("no CheckSignature");
                        this.context.Response.End();
                    }
                    SeatManage.SeatManageComm.WriteLog.Write("CheckSignature");
                    context.Response.Write(echostr);
                }
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
                //throw;
            }
        }

        private void Push(string OPENID, TemplateModel m)
        {
            try
            {
                string tempID = "WY4Z5Z0rigwhaSBl3onTma-VZC2sdTlHNviIXHV1n2s";//座位状态变更通知模板ID
                CommonTools tools = new CommonTools();
                string access_tocken = tools.IsExistAccess_Token("wx2b5a801cd0aa12c7", "42ac52963fadb13b2abff3a4c374888e");

               
                //TemplateModel m = new TemplateModel(first, keyword1, keyword2, keyword3, remark);
                m.touser = OPENID;
                m.template_id = tempID;
                m.url = "http://wechat.gxchuwei.com/User/SeatState";
                m.topcolor = "#FF0000";
                OpenApiResult result = tools.SendTemplateMessage(access_tocken, m);
                SeatManage.SeatManageComm.WriteLog.Write("msg_id" + result.msg_id + "error_code" + result.error_code + "error_msg" + result.error_msg);

            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
            }
        }


        private tb_User GetOpenIDByCardNo(string sid)
        {
            tb_User user = DbSession.Default.From<tb_User>().Where(tb_User._.CardNo == sid).ToFirst();
            return user;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}