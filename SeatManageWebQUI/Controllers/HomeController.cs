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
            ViewData["tree"] = str;
            return View();
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
                return "";
                //Response.Write(@"<script language='javascript'>alert('用户信息获取失败请重新登录！'); </script> ");
                //Response.Redirect(this.LogoutPage);
            }
          //  ShowUserInfo(LoginUser);
            List<SeatManage.ClassModel.SysMenuInfo> listSysMenu = LoginUser.UserMenus;

            StringBuilder menuString = new StringBuilder("[");

            foreach (SysMenuInfo item in listSysMenu)
            {
                menuString.Append("	{ \"id\":\""+ item.MenuID+ "\", \"parentId\":\"0\", \"name\":\""+item.MenuName+"\", \"isParent\": \"true\",\"backgroundPosition\":\"0px - 80px\",\"img\":\"./ skin / topIcons / icon01.png\"},");
                foreach (SysMenuInfo subItem in item.ChildMenu)
                {
                    menuString.Append("{ \"id\":\""+subItem.MenuID+"\", \"parentId\":\""+item.MenuID+"\", \"name\":\""+ subItem.MenuName+ "\",\"url\":\" / quickui / sample_skin / demo / oa_01.html\", \"target\":\"frmright\",\"icon\": \"./ skin / nav_icon_bg.png\",\"backgroundPosition\":\"0px - 128px\"},");
                }
            }
            string str = menuString.ToString().TrimEnd(',');
            str += "]";
            return str;
           


            //if (listSysMenu != null)
            //{
            //    foreach (SeatManage.ClassModel.SysMenuInfo list in listSysMenu)
            //    {
            //        FineUI.TreeNode node = new FineUI.TreeNode();
            //        node.Text = list.MenuName;
            //        node.Expanded = false;
            //        node.SingleClickExpand = true;
            //        TreeMenu.Nodes.Add(node);
            //        foreach (SeatManage.ClassModel.SysMenuInfo listChild in list.ChildMenu)
            //        {
            //            FineUI.TreeNode nodeChild = new FineUI.TreeNode();
            //            nodeChild.Text = listChild.MenuName;
            //            nodeChild.Expanded = false;
            //            nodeChild.NavigateUrl = "../" + listChild.MenuLink;
            //            node.Nodes.Add(nodeChild);
            //        }
            //    }
            //}
            //if (LoginUser.UserType == SeatManage.EnumType.UserType.Admin)
            //{
            //    houseTab.IFrameUrl = "../FunctionPages/Statistical/LibraryUsedStatistical.aspx";
            //}
            //else
            //{
            //    if (ConfigurationManager.AppSettings["ChangePassWord"] == "close")
            //    {
            //        btnPassword.Visible = false;
            //    }
            //    else
            //    {
            //        btnPassword.Visible = true;
            //    }
            //    houseTab.IFrameUrl = "../FunctionPages/Statistical/LibraryUsedStat.aspx";
            //}
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
    }
}