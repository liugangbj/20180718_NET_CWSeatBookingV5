using SeatManage.ClassModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers
{
    public class HomeController : BaseController
    {


        public ActionResult Index()
        {
            string str = LoadData();
            ViewBag.listTree = str;
            return View();
        }


        //unlockScreen
        public JsonResult UnlockScreen(string password)
        {
            JsonResult result = null;
            try
            {
                if (!IsLogin())
                {
                    Response.Write("<html><head><title>系统安全提示</title><script>alert('当前登录账户已经超时，请重新登录');location.href='/Login'</script></head><body></body></html>");
                    Response.End();
                }

                SeatManage.Bll.Users_ALL userinfocheck = new SeatManage.Bll.Users_ALL();
                string loginID = userinfocheck.CheckUser(LoginId, password);

                if (string.IsNullOrEmpty(loginID))
                {
                    result = Json(new { status = "no", message = "密码错误，请重新输入" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = Json(new { status = "yes", message = "登录成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                result = Json(new { status = "no", message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

            return result;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="loginid"></param>
        /// <returns></returns>
        private SeatManage.ClassModel.UserInfo GetUserInfo(string loginid)
        {
            SeatManage.ClassModel.UserInfo user = SeatManage.Bll.Users_ALL.GetUserInfo(loginid);
            if (user != null)
            {
                user.ReloID = SeatManage.Bll.Users_ALL.GetRoleID(loginid);
                user.UserRoomRight = SeatManage.Bll.T_SM_ManagerPotency.GetManangePotencyByLoginID(loginid);
                user.UserMenus = SeatManage.Bll.SysMenu.GetUserMenus(loginid);
                //获取全部对的阅览室权限
                if (loginid == "admin" || loginid == "user")
                {
                    List<SeatManage.ClassModel.ReadingRoomInfo> rightrooms = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);
                    if (user.UserRoomRight == null || rightrooms.Count != user.UserRoomRight.RightRoomList.Count)
                    {
                        user.UserRoomRight.RightRoomList.Clear();
                        foreach (SeatManage.ClassModel.ReadingRoomInfo room in rightrooms)
                        {
                            user.UserRoomRight.RightRoomList.Add(room);
                        }
                        SeatManage.Bll.Users_ALL.UpdateUserInfo(user);
                    }
                }
            }
            return user;
        }

        private string LoadData()
        {
            //获取用户信息
            SeatManage.ClassModel.UserInfo LoginUser = GetUserInfo(this.LoginId);
            if (LoginUser == null)
            {
                return "TimeOut";
            }
            //  ShowUserInfo(LoginUser);您好，欢迎访问系统应用！
            string msg = "您好[" + LoginUser.UserName + "],欢迎你回来！";
            ViewBag.welcomeMsg = msg;
            List<SeatManage.ClassModel.SysMenuInfo> listSysMenu = LoginUser.UserMenus;

            StringBuilder menuString = new StringBuilder("[");

            foreach (SysMenuInfo item in listSysMenu)
            {
                if (item.MenuID <= 3  ) continue; //不加载系统设置功能
                menuString.Append("	{ \"id\":\""+ item.MenuID+ "\", \"parentId\":\"0\", \"name\":\"" + item.MenuName+"\", \"isParent\": \"true\",\"backgroundPosition\":\"0px - 80px\",\"img\":\"./ skin / topIcons / icon01.png\"},");
                foreach (SysMenuInfo subItem in item.ChildMenu)
                {
                    menuString.Append("{ \"id\":\""+subItem.MenuID+"\", \"parentId\":\""+item.MenuID+"\", \"name\":\""+ subItem.MenuName+ "\",\"url\":\"/"+ subItem.MenuLink+"\", \"target\":\"frmright\",\"icon\": \"./ skin / nav_icon_bg.png\",\"backgroundPosition\":\"0px - 128px\"},");
                }
            }
            string str = menuString.ToString().TrimEnd(',');
            str += "]";
            return str;
       
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult LoginOut()
        {
            Session.Clear();
            Response.Write("<script>location.href='/Login'</script>");
            Response.End();
            return View("/Login");
        }
    }
}