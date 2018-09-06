using SeatManage.SeatManageComm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WeiXinMsgService.Model;

namespace AutoGetAccesstokenService
{
    class Program
    {
        private static TimeLoop timeLoop;//循环时间  
        static string loopInterval = ConfigurationManager.AppSettings["CheckTimes"];
        static string FilePath = ConfigurationManager.AppSettings["FilePath"];

        static string AppID = ConfigurationManager.AppSettings["AppID"];
        static string AppSecret = ConfigurationManager.AppSettings["AppSecret"];
        // static bool IsWork = false;

        static void Display(string msg)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + msg);
        }

        /// <summary>
        /// 通过appID和appsecret获取Access_token
        /// </summary>
        /// <returns></returns>
        private static WXAccess_token GetAccess_token()
        {
            string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppID + "&secret=" + AppSecret;
            WXAccess_token mode = new WXAccess_token();
            try
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                req.Method = "GET";

                using (WebResponse wr = req.GetResponse())
                {
                    HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    string content = reader.ReadToEnd();//在这里对Access_token 赋值  
                    content = content.Replace("{", "").Replace("}", "").Replace(",", ":").Replace("\"", ""); ;
                    string[] ar = content.Split(':');
                    mode.Token = ar[1].ToString();
                    mode.expires_in = int.Parse(ar[3].ToString());
                }
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
                throw;
            }
            return mode;
        }

        /// <summary>
        /// 将获取到的token写入xml
        /// </summary>
        private static void WriteXML()
        {
            try
            {
                string Token = string.Empty;
                DateTime YouXRQ;

                StreamReader str = new StreamReader(FilePath, System.Text.Encoding.UTF8);
                XmlDocument xml = new XmlDocument();
                xml.Load(str);
                str.Close();
                str.Dispose();
                WXAccess_token objWXAccess_token = GetAccess_token();
                Token = objWXAccess_token.Token;
                xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText = objWXAccess_token.Token;
                YouXRQ = DateTime.Now.AddSeconds(objWXAccess_token.expires_in);
                xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText = YouXRQ.ToString();
                xml.Save(FilePath);
            }
            catch (Exception ex)
            {
                Display(ex.ToString());
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
            }
        }

        private static void timeLoop_TimeTo(object sender, EventArgs e)
        {
            try
            {
                Display("获取Accesstoken开始");
                WriteXML();
                Display("更新完毕");
            }
            catch (Exception ex)
            {
                Display(ex.ToString());
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
            }
        }

        static void Main(string[] args)
        {
            Display("解释程序启动");
            WriteXML();
            int loopTime = 0;
            if (!int.TryParse(loopInterval, out loopTime))
            {
                SeatManage.SeatManageComm.WriteLog.Write("运行间隔时间获取失败，请检查是否配置了‘CheckTimes’项");
                Console.WriteLine("运行间隔时间获取失败，请检查是否配置了‘CheckTimes’项");
            }
            timeLoop = new TimeLoop(loopTime);
            timeLoop.TimeTo += new EventHandler(timeLoop_TimeTo);
            timeLoop.TimeStrat();
            Console.ReadLine();
        }
    }
}
