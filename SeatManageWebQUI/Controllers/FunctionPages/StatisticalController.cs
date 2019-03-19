using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class StatisticalController : BaseController
    {
        #region 进出人次统计——小时
        // GET: Statistical
        public ActionResult Index()
        {
            return RoomTripsOutInfo();
        }
        public ActionResult RoomTripsOutInfo()
        {
            List<SeatManage.ClassModel.LibraryInfo> libList = SeatManage.Bll.T_SM_Library.GetLibraryInfoList(null, null, null);
            ViewData["libList"] = libList;
            return View();
        }
        
        public JsonResult HourChartDataBinding(DateTime? begin, DateTime? end, string libNo = "", string roomStr = "", string cbselect = "", string cbleave = "",string cbonseat = "")
        {
            if (begin == null || end == null)
            {
                return Json(new { title = "日期错误", msg = "请选择日期！" });
            }
            if (end.Value.Date >= DateTime.Now.Date)
            {
                return Json(new { title = "日期错误",msg = "只能查询当天之前的数据！" });
            }
            if (begin.Value.Date > end.Value.Date)
            {
                return Json(new { title = "日期错误", msg = "开始时间必须小于结束时间！" }); ;
            }
            if (string.IsNullOrEmpty(libNo))
            {
                return Json(new { title = "图书馆错误", msg = "请选择图书馆！" }); 
            }
            StringBuilder sb = new StringBuilder();
            Dictionary<string, System.Data.DataTable> dataTables = new Dictionary<string,DataTable>();
            SeatManageWebV5.Code.ReadingRoomStatistics rrsta = new SeatManageWebV5.Code.ReadingRoomStatistics();
            List<string> roomList = new List<string>();
            if (!string.IsNullOrEmpty(roomStr))
            {
                var arr = roomStr.Split(',');
                if (arr != null && arr.Length > 0)
                {
                    foreach (var item in arr)
                    {
                        if (!string.IsNullOrEmpty(item)) roomList.Add(item);
                    }
                }
            }
            if (roomList.Count == 0)
            {
                List<string> libno = new List<string>();
                libno.Add(libNo);
                DataTable dt = rrsta.StatisticsHoursLibData(libno, begin.Value, end.Value);
                DataView dv = dt.DefaultView;
                dataTables.Add(libNo, dt);

                sb = GetHourChartJson(dt, cbselect, cbleave, cbonseat);
            }
            else
            {
                List<SeatManage.ClassModel.ReadingRoomInfo> rooms = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(roomList);
                foreach (SeatManage.ClassModel.ReadingRoomInfo room in rooms)
                {
                    SeatManage.ClassModel.ReadingRoomUsageStatistics roomUsage = rrsta.StatisticsHoursRoomData(room, begin.Value, end.Value);
                    DataTable dt = roomUsage.StatisticsData;
                    DataView dv = dt.DefaultView;
                    dataTables.Add(room.Name, dt);
                    sb.Append(GetHourChartJson(dt, cbselect, cbleave, cbonseat).ToString());
                }
            }
            Session["DataTables"] = dataTables;
            return Json(new { title = "", msg = "" ,data = sb.ToString()});
        }

        private StringBuilder GetHourChartJson(DataTable dt,string cbselect,string cbleave,string cbonseat)
        {
            StringBuilder sb = new StringBuilder();
            string Title = string.Format("{0}座位使用情况", DateTime.Now.ToLongDateString());

            sb.Append("{");
            sb.Append("title: {");
            sb.Append("text: '" + Title + "'");
            sb.Append("},");
            sb.Append(" tooltip: {},");
            sb.Append("legend: {");
            sb.Append(" data: ['入座人次', '离开人次', '在座人数']");
            sb.Append("},");
            sb.Append(" xAxis: {");
            sb.Append("axisLabel:");
            sb.Append(" {");
            sb.Append("interval: 0,");
            sb.Append("rotate:45,");
            sb.Append("margin:2,");
            sb.Append("textStyle:{");
            sb.Append("color:\"#222\"");
            sb.Append("}");
            sb.Append("},");

            sb.Append(" data: [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("'" + r["Hour"] + "'");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append(" },");

            sb.Append(" grid: { ");
            sb.Append("x: 40,");
            sb.Append("x2: 100,");
            sb.Append("y2: 150");
            sb.Append("},");

            sb.Append(" yAxis: {},");
            sb.Append(" series: [");
                       
            if (cbselect == "1")
            {
                sb.Append("{");
                sb.Append("name: '入座人次',");
                sb.Append("type: 'bar',");
                sb.Append(" data: [");
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("" + r["EnterCount"] + "");
                    sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("],");
                sb.Append("color: ['#0099cc']");
                sb.Append("},");
                
            }
            if (cbleave == "1")
            {
                sb.Append("{");
                sb.Append(" name: '离开人次',");
                sb.Append("type: 'bar',");
                sb.Append(" data: [");
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("" + r["OutCount"] + "");
                    sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("],");
                sb.Append("color: ['#ff0000']");
                sb.Append("},");
            }
            if (cbonseat == "1")
            {
                sb.Append("{");
                sb.Append("name: '在座人数',");
                sb.Append("type: 'bar',");
                sb.Append(" data: [");
                foreach (DataRow r in dt.Rows)
                {
                    sb.Append("" + r["SeatCount"] + "");
                    sb.Append(",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("],");
                sb.Append("color: ['#ffff00']");
                sb.Append("}");
            }
            sb.Append("]");
            sb.Append("}");
            return sb;
        }

        #endregion

        #region 获取阅览室列表CheckBox
        public string ReadingRoomBinding(string libNo)
        {
            var html = "<div style='width:800px'>";
            List<string> libNoList = new List<string>();
            libNoList.Add(libNo);
            List<SeatManage.ClassModel.ReadingRoomInfo> rooms = SeatManage.Bll.T_SM_ReadingRoom.GetReadingRooms(null, libNoList, null);
            if (rooms != null)
            {
                int i = 0;
                foreach (SeatManage.ClassModel.ReadingRoomInfo room in rooms)
                {
                    html += string.Format("<label><input type='CheckBox' ID='cbleave_{0}' value='{1}' class='check1'/>{2}</label>", i, room.No, room.Name);
                }
            }
            html += "</div>";
            return html;
        }
        #endregion

        #region 座位使用情况统计
        public ActionResult RoomSeatUseInfo()
        {
            List<SeatManage.ClassModel.LibraryInfo> libList = SeatManage.Bll.T_SM_Library.GetLibraryInfoList(null, null, null);
            ViewData["libList"] = libList;
            return View();
        }

        public JsonResult SeatUseChartDataBinding(DateTime? begin, DateTime? end, string libNo = "", string roomStr = "")
        {
            if (begin == null || end == null)
            {
                return Json(new { title = "日期错误", msg = "请选择日期！" });
            }
            if (end.Value.Date >= DateTime.Now.Date)
            {
                return Json(new { title = "日期错误", msg = "只能查询当天之前的数据！" });
            }
            if (begin.Value.Date > end.Value.Date)
            {
                return Json(new { title = "日期错误", msg = "开始时间必须小于结束时间！" }); ;
            }
            if (string.IsNullOrEmpty(libNo))
            {
                return Json(new { title = "图书馆错误", msg = "请选择图书馆！" });
            }
            SeatManageWebV5.Code.ReadingRoomStatistics rrsta = new SeatManageWebV5.Code.ReadingRoomStatistics();
            List<string> roomList = new List<string>();
            if (!string.IsNullOrEmpty(roomStr))
            {
                var arr = roomStr.Split(',');
                if (arr != null && arr.Length > 0)
                {
                    foreach (var item in arr)
                    {
                        if (!string.IsNullOrEmpty(item)) roomList.Add(item);
                    }
                }
            }

            List<SeatManage.ClassModel.ReadingRoomInfo> rooms = new List<SeatManage.ClassModel.ReadingRoomInfo>();
            if (roomList.Count == 0)
            {
                List<string> libno = new List<string>();
                libno.Add(libNo);
                rooms = SeatManage.Bll.T_SM_ReadingRoom.GetReadingRooms(null, libno, null);
            }
            else
            {
                rooms = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(roomList);
            }

            SeatManage.ClassModel.ReadingRoomUsageStatistics roomUsage = rrsta.StatisticsSelectAndLeave(rooms, begin.Value, end.Value);
            var data = "";
            Dictionary<string, DataTable> dataTables = new Dictionary<string, DataTable>();
            dataTables.Add("选座方式", roomUsage.SeatSelect);
            dataTables.Add("离开方式", roomUsage.SeatLeave);
            dataTables.Add("在座时长", roomUsage.SeatTime);
            Session["DataTables"] = dataTables;

            return Json(new { title = "", msg = "", data = data });
        }

        private StringBuilder GetSeatUseChartJson(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            string Title = string.Format("{0}座位使用情况", DateTime.Now.ToLongDateString());

            sb.Append("{");
            sb.Append("title: {");
            sb.Append("text: '" + Title + "'");
            sb.Append("},");
            sb.Append(" tooltip: {},");
            sb.Append("legend: {");
            sb.Append(" data: ['入座人次', '离开人次', '在座人数']");
            sb.Append("},");
            sb.Append(" xAxis: {");
            sb.Append("axisLabel:");
            sb.Append(" {");
            sb.Append("interval: 0,");
            sb.Append("rotate:45,");
            sb.Append("margin:2,");
            sb.Append("textStyle:{");
            sb.Append("color:\"#222\"");
            sb.Append("}");
            sb.Append("},");

            sb.Append(" data: [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("'" + r["Hour"] + "'");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append(" },");

            sb.Append(" grid: { ");
            sb.Append("x: 40,");
            sb.Append("x2: 100,");
            sb.Append("y2: 150");
            sb.Append("},");

            sb.Append(" yAxis: {},");
            sb.Append(" series: [");

            
            sb.Append("]");
            sb.Append("}");
            return sb;
        }
        #endregion

        #region 阅览室进出人次统计
        public ActionResult RoomTripsOutUseInfo()
        {
            return View();
        }
        #endregion

        #region 阅览室信息统计
        public ActionResult ReadingRoomUsageInfo()
        {
            return View();
        }
        #endregion

        #region 记录排行榜
        public ActionResult LogTopStatisticalV2()
        {
            return View();
        }
        #endregion
    }
}