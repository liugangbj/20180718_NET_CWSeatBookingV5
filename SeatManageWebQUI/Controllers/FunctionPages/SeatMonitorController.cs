using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class SeatMonitorController : Controller
    {
        // GET: SeatMonitor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MonitorGraphMode()
        {
            return View();
        }
        public ActionResult DeviceStatusInfo()
        {
            return View();
        }
    }
}