using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers
{
    public class BaseController: Controller
    {
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