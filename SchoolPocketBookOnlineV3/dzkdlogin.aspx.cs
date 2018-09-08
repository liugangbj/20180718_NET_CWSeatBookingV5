using SchoolPocketBookWeb.Code;
using SeatBespeakException;
using SeatManage.IPocketBespeakBllServiceV2;
using SeatManage.PocketBespeakBllServiceV2;
using SeatManage.SeatManageComm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SchoolPocketBookWeb
{
    public partial class dzkdlogin : BasePage
    {
        private IPocketBespeakBllService handler = new PocketBespeakBllService();

        private string Post(string url,string postData)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseString;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string openid = "oJeiev5E2IzQuKTPdC4D9db_sSXo";
                if (Request.Params["openid"] != null)
                {
                    if (Request.Params["openid"].ToString().Trim().Length > 0)
                    {
                        openid = Request.Params["openid"].ToString();
                    }
                }

                string strUrl = System.Configuration.ConfigurationManager.AppSettings["DzkdUrl"].ToString();
                string account = System.Configuration.ConfigurationManager.AppSettings["account"].ToString();
                string password = System.Configuration.ConfigurationManager.AppSettings["password"].ToString();
                string result = Post(strUrl, "account=" + account + "&password=" + password + "&openId=" + openid + "");//SeatManage.SeatManageComm.HttpRequest.Post(strUrl, "account=" + account + "&password=" + password + "&openid=" + openid + "");
                SeatManage.SeatManageComm.WriteLog.Write(result);
                if (result.Contains("uniNo") && result.Contains("userName")) //请求成功
                {
                    string loginOkUrl = "MainFunctionPage.aspx";
                    string cardNo = "";
                    DZKDJsonResultObj obj = JSONSerializer.Deserialize<DZKDJsonResultObj>(result);
                    if (loginHandle(obj.UniNo))
                    {
                        Response.Redirect(loginOkUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
            }


        }

        private bool loginHandle(string cardNo)
        {
            try
            {
                LoginUserInfo = handler.GetReaderInfoByCardNo(cardNo);
                return true;
            }
            catch (RemoteServiceLinkFailed ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
                Session.Clear();
                CookiesManager.RemoveCookies("userInfo");
                return false;
            }
            catch (Exception ex)
            {
                Session.Clear();
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
                CookiesManager.RemoveCookies("userInfo");
                return false;
            }
        }
    }
}