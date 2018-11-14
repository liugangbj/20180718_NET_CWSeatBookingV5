using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers
{
    public class BaseController: Controller
    {

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (!IsLogin())
            {
                Response.Write("<html><head><title>系统安全提示</title><script>alert('您没有权限进行当前操作，请重新选择用户登陆操作');location.href='/Login'</script></head><body></body></html>");
                Response.End();
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
    }
}