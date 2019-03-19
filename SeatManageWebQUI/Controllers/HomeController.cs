using SeatManage.ClassModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers
{
    public class HomeController : BaseController
    {
        List<SeatManage.ClassModel.LibraryInfo> libList = SeatManage.Bll.T_SM_Library.GetLibraryInfoList(null, null, null);

        public ActionResult Index()
        {
             
            string str = LoadData();
            ViewBag.listTree = str;
            return View();
        }

        public string GetCharts(string libID,string libText)
        {
            StringBuilder sb = new StringBuilder();
            // string Title = string.Format("{0}{1}座位使用情况", DateTime.Now.ToLongDateString(), libList[0].Name);
            if (libList != null && libList.Count > 0)
            {
                string libNo = string.IsNullOrEmpty(libID) ? libList[0].No : libID;
                string titleText = string.IsNullOrEmpty(libText) ? libList[0].Name : libText;

                string Title = string.Format("{0}{1}座位使用情况", DateTime.Now.ToLongDateString(), titleText);

                DataTable dt = SeatManageWebV5.Code.LogQueryHelper.LibrarySeatInfo(libNo);

                
                sb.Append("{");
                sb.Append("title: {");
                sb.Append("text: '" + Title + "'");
                sb.Append("},");
                sb.Append(" tooltip: {},");
                sb.Append("legend: {");
                sb.Append(" data: ['座位总数', '正在使用', '进出人次']");
                sb.Append("},");
                sb.Append(" xAxis: {");

                sb.Append("axisLabel:");
                sb.Append(" {");
                sb.Append("interval: 0,");
                sb.Append("rotate:45,");
                sb.Append("margin:2,");
                sb.Append("textStyle:{");
                sb.Append("color:\"#222\"");
                sb.Append("}");
                sb.Append("},");

                sb.Append(" data: [");// '衬衫', '羊毛衫', '雪纺衫','裤子','高跟鞋','袜子']");
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("'" + r["ReadingRoomName"] + "'");
                    sb.Append(",");
                }
                if (libList.Count > 0) sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                sb.Append(" },");

                sb.Append(" grid: { ");
                sb.Append("x: 40,");
                sb.Append("x2: 100,");
                sb.Append("y2: 150");
                sb.Append("},");

                sb.Append(" yAxis: {},");
                sb.Append(" series: [");
                sb.Append("{");
                sb.Append("name: '座位总数',");
                sb.Append("type: 'bar',");

                //sb.Append("data: [50, 20, 36, 10, 10, 20],");
                sb.Append(" data: [");
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("" + r["SeatAmount"] + "");
                    sb.Append(",");
                }
                if (libList.Count > 0) sb.Remove(sb.Length - 1, 1);
                sb.Append("],");

                sb.Append("color: ['#0099cc']");
                sb.Append("},");
                sb.Append("{");
                sb.Append(" name: '正在使用',");
                sb.Append("type: 'bar',");

                //  sb.Append("data: [60, 30, 10, 30, 20, 50],");
                sb.Append(" data: [");
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("" + r["SeatUsedAmount"] + "");
                    sb.Append(",");
                }
                if (libList.Count > 0) sb.Remove(sb.Length - 1, 1);
                sb.Append("],");

                sb.Append("color: ['#ff0000']");
                sb.Append("},");
                sb.Append("{");
                sb.Append("name: '进出人次',");
                sb.Append("type: 'bar',");

                // sb.Append("data: [90, 55, 44, 33, 22, 11],");
                sb.Append(" data: [");
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("" + r["PersonTimes"] + "");
                    sb.Append(",");
                }
                if (libList.Count > 0) sb.Remove(sb.Length - 1, 1);
                sb.Append("],");
                sb.Append("color: ['#ffff00']");
                sb.Append("}");
                sb.Append("]");
                sb.Append("}");

            }

     
            return sb.ToString() ;
        }

        public string GetLibsData()
        {   
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"list\":[");
            foreach (var item in libList)
            {
                sb.Append("{\"key\":\"" + item.Name + "\",\"value\":\"" + item.No + "\"},");
            }
            if (libList.Count > 0) sb.Remove(sb.Length - 1, 1);
            sb.Append("]}");

          
            return sb.ToString();
        }

        public ActionResult IndexData()
        {
            if (libList.Count > 0)
            {
                ViewBag.Title = string.Format("{0}{1}座位使用情况", DateTime.Now.ToLongDateString(), libList[0].Name);
            }
            else {
                ViewBag.Title = "暂无图书馆，请补充完善学校信息";//string.Format("{0}{1}座位使用情况", DateTime.Now.ToLongDateString(), libList[0].Name);
            }
            
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
            if (Session["casLogin"] != null && Session["casLogin"].ToString() == "1")
            {
                Session.Clear();
                return Redirect("http://ids.xmu.edu.cn/authserver/logout?service=https://lib.xmu.edu.cn/seat/xmulogin");
            }
            else
            {
                Session.Clear();
                Response.Write("<script>location.href='/Login'</script>");
                Response.End();
                return View("/Login");
            }
            
        }
    }
}