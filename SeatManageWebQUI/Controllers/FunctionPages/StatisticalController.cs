using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class StatisticalController : BaseController
    {
        // GET: Statistical
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RoomTripsOutInfo()
        {
            return View();
        }
        public ActionResult RoomSeatUseInfo()
        {
            return View();
        }
        public ActionResult RoomTripsOutUseInfo()
        {
            return View();
        }
        public ActionResult ReadingRoomUsageInfo()
        {
            return View();
        }
        public ActionResult LogTopStatisticalV2()
        {
            return View();
        }
    }
}