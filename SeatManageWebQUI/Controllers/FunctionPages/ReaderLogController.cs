using SeatManageWebV5.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class ReaderLogController : BaseController
    {
        // GET: ReaderLog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelectBespeakLog()
        {
            string nowDay = DateTime.Now.ToShortDateString();
            string after7Day = DateTime.Now.AddDays(7).ToShortDateString();
            ViewBag.nowDay = nowDay;
            ViewBag.after7Day = after7Day;
            return View();
        }

        public string GetBespeakEnumKeyValue()
        {
            List<BespeakEnumKey_Value> list = new List<BespeakEnumKey_Value>();
            list.Add(new BespeakEnumKey_Value() { BespeakState = SeatManage.SeatManageComm.SeatComm.ConvertBookingStatus(SeatManage.EnumType.BookingStatus.None), Value = ((int)SeatManage.EnumType.BookingStatus.None).ToString() });
            list.Add(new BespeakEnumKey_Value() { BespeakState = SeatManage.SeatManageComm.SeatComm.ConvertBookingStatus(SeatManage.EnumType.BookingStatus.Waiting), Value = ((int)SeatManage.EnumType.BookingStatus.Waiting).ToString() });
            list.Add(new BespeakEnumKey_Value() { BespeakState = SeatManage.SeatManageComm.SeatComm.ConvertBookingStatus(SeatManage.EnumType.BookingStatus.Confinmed), Value = ((int)SeatManage.EnumType.BookingStatus.Confinmed).ToString() });
            list.Add(new BespeakEnumKey_Value() { BespeakState = SeatManage.SeatManageComm.SeatComm.ConvertBookingStatus(SeatManage.EnumType.BookingStatus.Cencaled), Value = ((int)SeatManage.EnumType.BookingStatus.Cencaled).ToString() });

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"list\":[");
            foreach (var item in list)
            {
                sb.Append("{\"key\":\"" + item.Value + "\",\"value\":\"" + item.BespeakState + "\"},");
            }
            sb.Append("]}");

            return sb.ToString();
        }

        public ActionResult SelectEnterOutLog()
        {
            return View();
        }

        public ActionResult SelectViolateDiscipline()
        {
            return View();
        }

        public ActionResult SelectBlacklist()
        {
            return View();
        }
        public ActionResult SelectNoticeLog()
        {
            return View();
        }



    }
}