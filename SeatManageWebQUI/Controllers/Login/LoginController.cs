using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.Login
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("Login");
        }


        public JsonResult CheckLogin(string username,string password)
        {
            JsonResult result = null;
            string loginID = username;
            string Password = password;

            try
            {
                SeatManage.Bll.Users_ALL userinfocheck = new SeatManage.Bll.Users_ALL();
                loginID = userinfocheck.CheckUser(loginID, Password);

                if (string.IsNullOrEmpty(loginID))
                {
                    result = Json(new { status = "no", message = "用户或密码错误，请重新输入" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Session["LoginID"] = loginID;
                    result = Json(new { status = "yes", message = "登录成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                result = Json(new { status = "no", message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

            return result;
        }

    }
}