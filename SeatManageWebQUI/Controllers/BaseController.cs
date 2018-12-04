using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers
{
    public class BaseController: Controller
    {

        static string serverName;
        static string httpReferer;
        static string httpHost;
        static string loginId;
        static string url;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (!IsLogin())
            {
                Response.Write("<html><head><title>系统安全提示</title><script>alert('您没有权限进行当前操作，请重新选择用户登陆操作');location.href='/Login'</script></head><body></body></html>");
                Response.End();
            }

            //if (Session["LoginID"] == null)
            //{
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请先登录！');parent.location.href='" + LoginPage + "';", true);
            //}
            //else
            //{
            //    loginId = Session["LoginID"].ToString();
            //}
            
            url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString();
            if (Request.ServerVariables["SERVER_NAME"] != null && Request.ServerVariables["HTTP_REFERER"] != null && Request.ServerVariables["HTTP_HOST"] != null)
            {
                serverName = Request.ServerVariables["SERVER_NAME"].Trim();//服务器名称 
                httpReferer = Request.ServerVariables["HTTP_REFERER"].Trim();//http接收的名称 
                httpHost = Request.ServerVariables["HTTP_HOST"].Trim();//类似这样的格式www.ccopus.com 
            }
            else
            {
                serverName = "";
                httpReferer = "";
                httpHost = "";
            }

        }

        protected bool IsLogin()
        {
            if (LoginId != null)
            {
                return true;
            }
            return false;
        }


        protected string LoginId
        {
            get
            {
                if (Session["LoginID"] != null)
                {
                    string loginId = Session["LoginID"].ToString();
                    return loginId;
                }
                else
                {
                  //  Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请先登录！');parent.location.href='" + LoginPage + "';", true);
                    return null;
                }
            }
            set { Session["LoginID"] = value; }
        }

        public static bool OpVerifiction()
        {
            //string s1 = Request.ServerVariables["SERVER_NAME"].Trim();//服务器名称 
            //if (!IsPostBack)
            //{
            bool result = false;
            if (!string.IsNullOrEmpty(serverName) && !string.IsNullOrEmpty(httpReferer) && !string.IsNullOrEmpty(httpHost))
            {
                //string s2 = Request.ServerVariables["HTTP_REFERER"].Trim();//http接收的名称 
                //string s4 = Request.ServerVariables["HTTP_HOST"].Trim();//类似这样的格式www.ccopus.com 
                int count = httpHost.Length + 1 + 7;
                string strFlorms = "home/index";
               // string strFlorms = "Florms/FormSYS.aspx";//home/index
                string strGetUrl = httpReferer.Substring(count).ToLower();
                if (string.IsNullOrEmpty(strGetUrl))
                {
                    result = true;
                }
                if (strGetUrl != strFlorms.ToLower().Trim() && strGetUrl != "home/index")
                {
                    result = false;
                    WriteLogs(url);
                    //Response.Write("警告！你的IP已经被记录!不要使用敏感字符！");
                    //Response.End();
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                WriteLogs(url);
            }
            return result;
        }

        public static void WriteLogs(string geturl)
        {
            SeatManage.SeatManageComm.WriteLog.Write(string.Format("用户通过非法登录访问网站，访问页面地址为：{0},用户IP地址为：{1},登录名为：{2}", geturl, GetIP(), loginId));
        }

        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = String.Empty;
            result = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                return "127.0.0.1";
            }
            return result;
        }
    }
}