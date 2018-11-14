using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class RegulationRulesSettingController : Controller
    {
        // GET: RegulationRulesSetting
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DeviceInfo()
        {
            return View();
        }
        public ActionResult BlacklistInfo()
        {
            return View();
        }
        public ActionResult SyncSet()
        {
            return View();
        }
        public ActionResult AccessSetting()
        {
            return View();
        }
        public ActionResult PecketWebSetting()
        {
            return View();
        }
        public ActionResult PushMsgSetting()
        {
            return View();
        }
    }
}