using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class SystemSetController : Controller
    {
        // GET: SystemSet
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FunctionPagesManage()
        {
            return View();
        }

        public ActionResult MenuManage()
        {
            return View();
        }
    }
}