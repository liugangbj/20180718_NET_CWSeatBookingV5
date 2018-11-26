using SeatManage.ClassModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class UsersManageController : BaseController
    {
        // GET: UsersManage
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult RemoveUser(string LoginId)
        {
            JsonResult result = null;
            SeatManage.ClassModel.UserInfo user = new SeatManage.ClassModel.UserInfo();
            user.LoginId = LoginId;
            if (user.LoginId == "admin" || user.LoginId == "user" || user.LoginId == "reader")
            {
                return Json(new { status = "no", message = "删除失败["+LoginId+"]是保留用戶，不能刪除" }, JsonRequestBehavior.AllowGet);
            }
            if (!SeatManage.Bll.Users_ALL.DeleteUser(user))
            {
                result = Json(new { status = "no", message = "删除失败" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = Json(new { status = "yes", message = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
           return result;
        }


        public JsonResult SaveOrUpdate()
        {
            JsonResult result = null;
            List<SeatManage.ClassModel.SysRolesDicInfo> rolelist = SeatManage.Bll.SysRolesDic.GetRoleList(null, null);
            List<SeatManage.ClassModel.ReadingRoomInfo> roomlist = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);
            string LoginId = Request.Params["LoginId"];
            string txtUserName = Request.Params["txtUserName"];
            string txtPassword = Request.Params["txtPassword"];
            string txtRemark = Request.Params["txtRemark"];
            bool IsUsing = Request.Params["IsUsing"] == null ? false : true;

            bool saveOrUpdateIsOk = false;
            if (Request.Params["op"] == "add") //新增
            {
                SeatManage.ClassModel.UserInfo user = new SeatManage.ClassModel.UserInfo();
                user.LoginId = LoginId;
                user.UserName = txtUserName;
                user.Password = SeatManage.SeatManageComm.MD5Algorithm.GetMD5Str32(txtPassword);   // user.Password = SeatManage.SeatManageComm.MD5Algorithm.GetMD5Str32(txtPassword.Text.Trim());
                user.Remark = txtRemark;
                user.IsUsing = IsUsing ? SeatManage.EnumType.LogStatus.Valid : SeatManage.EnumType.LogStatus.Fail;
                user.ReloID = new List<int>();
                foreach (var role in rolelist)
                {
                    if (Request.Params["role_" + role.RoleID] != null)
                    {
                        user.ReloID.Add(int.Parse(role.RoleID));
                    }
                    
                }
                user.UserRoomRight = new ManagerPotency();
                user.UserRoomRight.RightRoomList = new List<ReadingRoomInfo>();
                user.UserRoomRight.LoginID = LoginId;
                foreach (var room in roomlist)
                {
                    if (Request.Params["room_" + room.No] != null)
                    {
                        user.UserRoomRight.RightRoomList.Add(new SeatManage.ClassModel.ReadingRoomInfo() { No = room.No });
                    } 
                }
                saveOrUpdateIsOk = SeatManage.Bll.Users_ALL.AddNewUser(user);

            }
            else //编辑
            {

                SeatManage.ClassModel.UserInfo user = SeatManage.Bll.Users_ALL.GetUserInfo(LoginId);
                user.LoginId = LoginId;
                user.UserName = txtUserName;
                user.Password = SeatManage.SeatManageComm.MD5Algorithm.GetMD5Str32(txtPassword);
                user.Remark = txtRemark;
                user.IsUsing = IsUsing ? SeatManage.EnumType.LogStatus.Valid : SeatManage.EnumType.LogStatus.Fail;
                user.ReloID.Clear();
                foreach (var role in rolelist)
                {
                    if (Request.Params["role_" + role.RoleID] != null)
                    {
                        user.ReloID.Add(int.Parse(role.RoleID));
                    }

                }
                user.UserRoomRight.RightRoomList.Clear();
                user.UserRoomRight.LoginID = LoginId;
                foreach (var room in roomlist)
                {
                    if (Request.Params["room_" + room.No] != null)
                    {
                        user.UserRoomRight.RightRoomList.Add(new SeatManage.ClassModel.ReadingRoomInfo() { No = room.No });
                    }
                }
                saveOrUpdateIsOk = SeatManage.Bll.Users_ALL.UpdateUserInfo(user);
            }

            result = saveOrUpdateIsOk ? Json(new { status = "yes", message = "保存成功" }, JsonRequestBehavior.AllowGet) : Json(new { status = "no", message = "保存失败" }, JsonRequestBehavior.AllowGet);
            return result;
        }

        public ActionResult AddOrEdit(string op,string LoginId)
        {
            SeatManage.ClassModel.UserInfo user = new SeatManage.ClassModel.UserInfo();
            StringBuilder roleHtml = new StringBuilder();
            StringBuilder roomHtml = new StringBuilder();

            List<SeatManage.ClassModel.SysRolesDicInfo> rolelist = SeatManage.Bll.SysRolesDic.GetRoleList(null, null);
            List<SeatManage.ClassModel.ReadingRoomInfo> roomlist = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);


            if (op == "add")
            {
                foreach (var role in rolelist)
                {
                    roleHtml.Append("<input type=\"checkbox\"  ID=\"role_" + role.RoleID + "\" name=\"role_" + role.RoleID + "\" /><label for=\"role_" + role.RoleID + "\" class=\"hand\">" + role.RoleName + "</label>");
                }
                foreach (ReadingRoomInfo room in roomlist)
                {
                    roomHtml.Append("<input type=\"checkbox\"  ID=\"room_" + room.No + "\" name=\"room_" + room.No + "\" /><label for=\"room_" + room.No + "\" class=\"hand\">" + room.Name + "</label>");
                }
            }
            else if(op == "edit")
            {
                user = SeatManage.Bll.Users_ALL.GetUserInfo(LoginId);
                if (user != null)
                {
                    user.ReloID = SeatManage.Bll.Users_ALL.GetRoleID(LoginId);
                    user.UserRoomRight = SeatManage.Bll.T_SM_ManagerPotency.GetManangePotencyByLoginID(LoginId);
                }
                foreach (var role in rolelist)
                {
                    string str = "<input type=\"checkbox\"  ID=\"role_" + role.RoleID + "\" name=\"role_" + role.RoleID + "\" /><label for=\"role_" + role.RoleID + "\" class=\"hand\">" + role.RoleName + "</label>";
                    foreach (int userRole in user.ReloID)
                    {
                        if(userRole == int.Parse(role.RoleID))
                        {
                             str = "<input type=\"checkbox\" checked=\"true\"  ID=\"role_" + role.RoleID + "\" name=\"role_" + role.RoleID + "\" /><label for=\"role_" + role.RoleID + "\" class=\"hand\">" + role.RoleName + "</label>";
                        }
                    }
                    roleHtml.Append(str);
                }
                foreach (var room in roomlist)
                {
                    string str = "<input type=\"checkbox\"  ID=\"room_" + room.No + "\" name=\"room_" + room.No + "\" /><label for=\"room_" + room.No + "\" class=\"hand\">" + room.Name + "</label>";
                    foreach (ReadingRoomInfo rr in user.UserRoomRight.RightRoomList)
                    {
                        if (rr.No == room.No)
                        {
                            str = "<input type=\"checkbox\" checked=\"true\"  ID=\"room_" + room.No + "\" name=\"room_" + room.No + "\" /><label for=\"room_" + room.No + "\" class=\"hand\">" + room.Name + "</label>";
                        }
                    }
                    roomHtml.Append(str);
                }
            }

            ViewBag.roleHtml = roleHtml.ToString();
            ViewBag.roomHtml = roomHtml.ToString();
            ViewBag.User = user;
            ViewBag.op = op;
            return View();
        }

        public ActionResult UserInfoQuery(string strWhere)
        {
            //JsonResult result = null;
            List<SeatManage.ClassModel.UserInfo> userlist = SeatManage.Bll.Users_ALL.GetUsers();
            if (!string.IsNullOrEmpty(strWhere))
            {
                userlist = userlist.Where(x => x.LoginId.Contains(strWhere) || x.UserName.Contains(strWhere)).ToList<UserInfo>();
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (UserInfo item in userlist)
            {
                string UserTypeStr = "";
                if (item.UserType == SeatManage.EnumType.UserType.Admin)
                {
                    UserTypeStr = "管理员";

                }
                else if (item.UserType == SeatManage.EnumType.UserType.Reader)
                {
                    UserTypeStr = "读者";
                }
                else
                {
                    UserTypeStr = "未知";
                }

                string IsUsingStr = "";
                if (item.IsUsing == SeatManage.EnumType.LogStatus.Valid) IsUsingStr = "有效";
                if (item.IsUsing == SeatManage.EnumType.LogStatus.Fail) IsUsingStr = "无效";
                if (item.IsUsing == SeatManage.EnumType.LogStatus.None) IsUsingStr = "未知";
                sb.Append("{\"LoginId\": '" + item.LoginId + "',\"UserName\": \"" + item.UserName + "\",\"UserType\": \"" + UserTypeStr + "\",\"IsUsing\": \"" + IsUsingStr + "\",\"Remark\": \"" + item.Remark + "\"}");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append("}");
            string data = sb.ToString();
            ViewBag.Data = data;
            ViewBag.strWhere = strWhere;
            //result = Json(new { status = "yes", message = "模糊查询成功" }, JsonRequestBehavior.AllowGet);
            return View("UserInfo");
        }

        public ActionResult UserInfo()
        {
            List<SeatManage.ClassModel.UserInfo> userlist = SeatManage.Bll.Users_ALL.GetUsers();
            //if (!string.IsNullOrEmpty(searchInput))
            //{
            //    userlist = userlist.Where(x => x.LoginId.Contains(searchInput) || x.UserName.Contains(searchInput)).ToList<UserInfo>();
            //}

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (UserInfo item in userlist)
            {
                string UserTypeStr = "";
                if (item.UserType== SeatManage.EnumType.UserType.Admin)
                {
                    UserTypeStr = "管理员";

                }else if(item.UserType== SeatManage.EnumType.UserType.Reader)
                {
                    UserTypeStr = "读者";
                }else
                {
                    UserTypeStr = "未知";
                }

                string IsUsingStr = "";
                if (item.IsUsing == SeatManage.EnumType.LogStatus.Valid) IsUsingStr = "有效";
                if (item.IsUsing == SeatManage.EnumType.LogStatus.Fail) IsUsingStr = "无效";
                if (item.IsUsing == SeatManage.EnumType.LogStatus.None) IsUsingStr = "未知";
                sb.Append("{\"LoginId\": '" + item.LoginId + "',\"UserName\": \"" + item.UserName + "\",\"UserType\": \"" + UserTypeStr + "\",\"IsUsing\": \"" + IsUsingStr + "\",\"Remark\": \"" + item.Remark + "\"}");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append("}");
            string data = sb.ToString();
            ViewBag.Data = data;
            return View();
        }
        public ActionResult RoleManage()
        {
            return View();
        }
    }
}