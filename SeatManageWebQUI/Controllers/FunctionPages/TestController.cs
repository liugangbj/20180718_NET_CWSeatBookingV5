using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestLoading()
        {

            System.Threading.Thread.Sleep(5000);
            return View();
        }
    }
}