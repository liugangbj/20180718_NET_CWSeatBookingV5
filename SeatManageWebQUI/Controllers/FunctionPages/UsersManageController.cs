using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class UsersManageController : Controller
    {
        // GET: UsersManage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserInfo()
        {
            return View();
        }
        public ActionResult RoleManage()
        {
            return View();
        }
    }
}