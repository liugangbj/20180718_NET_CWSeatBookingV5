using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class SeatBespeakController : Controller
    {
        // GET: SeatBespeak
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BespeakSelfSeat()
        {
            return View();
        }
        public ActionResult BespeakSeat()
        {
            return View();
        }
        public ActionResult BespeakNowDaySeat()
        {
            return View();
        }
        public ActionResult BespeakProcess()
        {
            return View();
        }
    }
}