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
        public ActionResult UserInfo(string searchInput)
        {
            List<SeatManage.ClassModel.UserInfo> userlist = SeatManage.Bll.Users_ALL.GetUsers();
            if (!string.IsNullOrEmpty(searchInput))
            {
                userlist = userlist.Where(x => x.LoginId.Contains(searchInput) || x.UserName.Contains(searchInput)).ToList<UserInfo>();
            }

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