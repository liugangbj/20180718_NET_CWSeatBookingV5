using Newtonsoft.Json;
using SeatManage.ClassModel;
using SeatManageWebV5.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class SeatBespeakController : BaseController
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

        /// <summary>
        /// 绑定图书馆下拉列表
        /// </summary>
        public string BindLibaray()
        {
            List<SeatManage.ClassModel.LibraryInfo> listLibrary = new List<SeatManage.ClassModel.LibraryInfo>();
            listLibrary = SeatManage.Bll.T_SM_Library.GetLibraryInfoList(null, null, null);
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"list\":[");
            foreach (var item in listLibrary)
            {
                sb.Append("{\"key\":\""+item.Name+"\",\"value\":\""+ item.No + "\"},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]}");
            return sb.ToString();
        }

        private bool dateBespeak(SeatManage.ClassModel.SeatBespeakSet set, DateTime nowDate, string SelectedDate)
        {

            DateTime selectedDate = DateTime.Parse(SelectedDate);//this.dpStartDate.SelectedDate.Value;
            TimeSpan span = selectedDate.Date - nowDate.Date;
            //判断当天是否大于选择的日期
            if (span.Days > set.BespeakBeforeDays)
            {
                return false;
            }
            for (int i = 0; i < set.NoBespeakDates.Count; i++)
            {
                try
                {
                    DateTime beginDate = DateTime.Parse(set.NoBespeakDates[i].BeginTime);
                    DateTime endDate = DateTime.Parse(set.NoBespeakDates[i].EndTime);
                    if (SeatManage.SeatManageComm.DateTimeOperate.DateAccord(beginDate, endDate, selectedDate) || selectedDate.CompareTo(beginDate) == 0 || selectedDate.CompareTo(endDate) == 0)
                    {//如果当前时间符合某个不可预约的规则，则直接返回false，不可预约
                        return false;
                    }
                }
                catch
                {//日期转换遇到异常，则忽略 
                }
            }
            return true;
        }

        private bool timeCanBespeak(SeatManage.ClassModel.SeatBespeakSet set, DateTime nowDate)
        {
            try
            {
                DateTime beginTime = DateTime.Parse(string.Format("{0} {1}", nowDate.ToShortDateString(), set.CanBespeatTimeSpace.BeginTime));
                DateTime endTime = DateTime.Parse(string.Format("{0} {1}", nowDate.ToShortDateString(), set.CanBespeatTimeSpace.EndTime));
                if (SeatManage.SeatManageComm.DateTimeOperate.DateAccord(beginTime, endTime, nowDate))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public JsonResult CheckBooking(string SelectedDate,string roomNo,string canBespeakAmountStr)
        {
            JsonResult result = null;
            DateTime nowDate = SeatManage.Bll.ServiceDateTime.Now;
            DateTime selectedDate = DateTime.Parse(SelectedDate);//this.dpStartDate.SelectedDate.Value;
            ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(roomNo);
            string roomSet = room.Setting.ToString();
            if (string.IsNullOrEmpty(roomSet))
            {
                result = Json(new { status = "no", message = "该阅览室没有配置"  }, JsonRequestBehavior.AllowGet);
            }
            else if (selectedDate.CompareTo(nowDate) <= 0)
            {
                result = Json(new { status = "no", message = "日期选择错误" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    SeatManage.ClassModel.ReadingRoomSetting set = new SeatManage.ClassModel.ReadingRoomSetting(roomSet);
                    int canBespeakAmount = int.Parse(canBespeakAmountStr);
                    if (!set.SeatBespeak.Used || set.RoomOpenSet.UninterruptibleModel)
                    {
                        result = Json(new { status = "no", message = "阅览室没有开放预约" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (!dateBespeak(set.SeatBespeak, nowDate, SelectedDate))
                    {
                        result = Json(new { status = "no", message = "该日期座位不能预约" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (!timeCanBespeak(set.SeatBespeak, nowDate))
                    {
                        result = Json(new { status = "no", message = string.Format("预约时间为：{0}到{1}", set.SeatBespeak.CanBespeatTimeSpace.BeginTime, set.SeatBespeak.CanBespeatTimeSpace.EndTime) }, JsonRequestBehavior.AllowGet);
                    }
                    else if (canBespeakAmount <= 0)
                    {
                        result = Json(new { status = "no", message = "座位已被全部预约" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        
                        string urlParametersStr = SeatManage.SeatManageComm.AESAlgorithm.DESEncode(string.Format("roomNo={0}&date={1}", roomNo, selectedDate.ToBinary()));
                        result = Json(new { status = "yes", message = "预约座位", urlParameters=urlParametersStr }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    
                    result = Json(new { status = "no", message = ex.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            //  var jsonstr = JsonConvert.SerializeObject(result);
            return result;
        }

        public string BindingGrid(string libno,string SelectedDate)
        {
            StringBuilder sb = new StringBuilder();
          //  string libno = ddlLibrary.SelectedValue;
            DateTime date = DateTime.Parse(SelectedDate);
            DataTable dt = LogQueryHelper.BespeakRoomList(date, libno);

            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("{\"roomNum\": '" + r["roomNum"] + "',\"roomName\": \"" + r["roomName"] + "\",\"libraryName\": \"" + r["libraryName"] + "\",\"CanBespeakAmcount\": \"" + r["CanBespeakAmcount"] + "\",\"SurplusBespeskAmcount\": \"" + r["SurplusBespeskAmcount"] + "\"}");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append("}");

            return sb.ToString();
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