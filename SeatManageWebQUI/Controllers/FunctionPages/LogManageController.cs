using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class LogManageController : Controller
    {
        // GET: LogManage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EnterOutLog()
        {
            return View();
        }
        public ActionResult BespeakLog()
        {
            return View();
        }
        public ActionResult ViolateDiscipline()
        {
            return View();
        }
        public ActionResult Blacklist()
        {
            return View();
        }
    }
}