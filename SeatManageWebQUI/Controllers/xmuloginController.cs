using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers
{
    public class xmuloginController : Controller
    {
        // GET: xmulogin
        public ActionResult Index(string ticket = "")
        {
            if (!string.IsNullOrEmpty(ticket))//拿到票据 去校验ticket
            {
                var CasCallbackUrl = GetAppSetting("CasCallbackUrl");

                string url = "http://ids.xmu.edu.cn/authserver/serviceValidate?ticket=" + ticket + "&service=" + CasCallbackUrl;
                var xml = GetXML(url);

                if (!string.IsNullOrEmpty(xml))//拿到用户xml
                {
                    SeatManage.Bll.Users_ALL userinfocheck = new SeatManage.Bll.Users_ALL();
                    var index1 = xml.IndexOf("<cas:user>");
                    var index2 = xml.IndexOf("</cas:user>");
                    var loginID = xml.Substring(index1 + 10, index2 - index1 - 10);
                    var userInfo = SeatManage.Bll.Users_ALL.GetUserInfo(loginID);
                    if (userInfo != null)
                    {
                        //登录
                        Session["LoginID"] = loginID;
                        Session["casLogin"] = "1";
                        return RedirectToAction("index", "home");
                    }
                    else
                    {
                        TempData["msg"] = "用户未注册！";
                        return RedirectToAction("index", "login");
                    }
                }
                else
                {
                    TempData["msg"] = "登录失败！";
                    return RedirectToAction("index", "login");
                }
            }
            
            return RedirectToAction("index", "login");
        }

        public string GetXML(string url)
        {
            var res = "";
            try
            {
                HttpWebRequest wbRequest = (HttpWebRequest)WebRequest.Create(url);
                wbRequest.Method = "GET";
                HttpWebResponse wbResponse = (HttpWebResponse)wbRequest.GetResponse();
                using (Stream responseStream = wbResponse.GetResponseStream())
                {
                    using (StreamReader sReader = new StreamReader(responseStream))
                    {
                        res = sReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        public string GetAppSetting(string name, string defaultValue = "")
        {
            var res = ConfigurationSettings.AppSettings[name];
            return string.IsNullOrEmpty(res) ? defaultValue : res;
        }
    }
}