using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class SchoolInfoManageController : Controller
    {
        // GET: SchoolInfoManage
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SchoolInfo()
        {
            return View();
        }
        public ActionResult LibraryInfo()
        {
            return View();
        }
        public ActionResult ReadingRoomInfo()
        {
            return View();
        }
    }
}