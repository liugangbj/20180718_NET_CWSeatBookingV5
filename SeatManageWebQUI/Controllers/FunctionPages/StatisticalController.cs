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
            OpVerifiction();
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

                sb = GetHourChartJson(dt);
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
                    sb.Append(GetHourChartJson(dt).ToString());
                }
            }
            Session["DataTables"] = dataTables;
            return Json(new { title = "", msg = "" ,data = sb.ToString()});
        }

        private StringBuilder GetHourChartJson(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            string Title = string.Format("座位使用情况", DateTime.Now.ToLongDateString());

            sb.Append("{title: {text: '" + Title + "'}, tooltip: {},legend: { data: ['入座人次', '离开人次', '在座人数']}, xAxis: {axisLabel: {interval: 0,rotate:45,margin:2,textStyle:{color:\"#222\"}},");

            sb.Append(" data: [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("'" + r["Hour"] + "'");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("] }, grid: { x: 40,x2: 100,y2: 150}, yAxis: {}, series: [{name: '入座人次',type: 'bar',");
            sb.Append(" data: [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("" + r["EnterCount"] + "");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("],color: ['#0099cc']},{ name: '离开人次',type: 'bar',");
            sb.Append(" data: [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("" + r["OutCount"] + "");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("],color: ['#ff0000']},{name: '在座人数',type: 'bar',");
            sb.Append(" data: [");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("" + r["SeatCount"] + "");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("],color: ['#ffff00']}]}");
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
                    i++;
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
            /*if (end.Value.Date >= DateTime.Now.Date)
            {
                return Json(new { title = "日期错误", msg = "只能查询当天之前的数据！" });
            }*/
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
            var data1 = GetSeatUseChartJson(roomUsage.SeatSelect, "SelectType");
            var data2 = GetSeatUseChartJson(roomUsage.SeatLeave, "LeaveType");
            var data3 = GetSeatUseChartJson(roomUsage.SeatTime, "SeatTime");
            Dictionary<string, DataTable> dataTables = new Dictionary<string, DataTable>();
            dataTables.Add("选座方式", roomUsage.SeatSelect);
            dataTables.Add("离开方式", roomUsage.SeatLeave);
            dataTables.Add("在座时长", roomUsage.SeatTime);
            Session["DataTables"] = dataTables;

            return Json(new { title = "", msg = "", data1 = data1, data2 = data2, data3 = data3 });
        }

        private string GetSeatUseChartJson(DataTable dt,string colName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (DataRow r in dt.Rows)
            {
                sb.Append("{name:'" + r[colName] + "  " + r["Count"] + "',value:'" + r["Count"] + "'},");
            }
            sb.Append("]");
            return sb.ToString();
        }
        #endregion

        #region 阅览室进出人次统计
        public ActionResult RoomTripsOutUseInfo()
        {
            List<SeatManage.ClassModel.LibraryInfo> libList = SeatManage.Bll.T_SM_Library.GetLibraryInfoList(null, null, null);
            ViewData["libList"] = libList;
            return View();
        }

        public JsonResult OutChartDataBinding(DateTime? begin, DateTime? end, string libNo = "",string rbltype = "",string rbleatype = "", string roomStr = "")
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
            Dictionary<string, DataTable> dataTables = new Dictionary<string, DataTable>();
            DataTable dtx = new DataTable();
            dtx.Columns.Add("EnterOutCount", typeof(int));
            dtx.Columns.Add("AttendanceCount", typeof(string));
            dtx.Columns.Add("Room", typeof(string));
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
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            if (roomList.Count == 0)
            {
                List<string> libno = new List<string>();
                libno.Add(libNo);
                SeatManage.ClassModel.ReadingRoomUsageStatistics libUsage = rrsta.StatisticsDayLibData(libno, begin.Value, end.Value, ((SeatManage.ClassModel.StatisticsType)int.Parse(rbltype)));
                DataTable dt = libUsage.StatisticsData;
                DataView dv = dt.DefaultView;
                dataTables.Add(libNo, dt);
                string Title = string.Format("阅览室进出人次统计");
                var ztext = "";
                var colName = "";
                if (rbleatype == "0" || rbleatype == "EnterOutCount")
                {
                    ztext = "进出人次";
                    colName = "EnterOutCount";
                }
                else
                {
                    ztext = "上座率";
                    colName = "Attendance";
                }

                sb1.Append("{title: {text: '" + Title + "'}, tooltip: {},legend: { data: ['"+ ztext + "' ]}, xAxis: {axisLabel: {interval: 0,rotate:45,margin:2,textStyle:{color:\"#222\"}},");

                sb1.Append(" data: [");
                foreach (DataRow r in dt.Rows)
                {
                    sb1.Append("'" + r["Date"] + "'");
                    sb1.Append(",");
                }
                sb1.Remove(sb1.Length - 1, 1);
                sb1.Append("] }, grid: { x: 40,x2: 100,y2: 150}, yAxis: {}, series: [{name: '"+ ztext + "',type: 'bar',");
                sb1.Append(" data: [");
                foreach (DataRow r in dt.Rows)
                {
                    sb1.Append("" + r[colName] + "");
                    sb1.Append(",");
                }
                sb1.Remove(sb1.Length - 1, 1);
                sb1.Append("],color: ['#ffff00']}]}");
                
                DataRow dr = dtx.NewRow();
                dr["Room"] = libNo;
                dr["EnterOutCount"] = libUsage.EnterOurCount;
                dr["AttendanceCount"] = (libUsage.Attendance * 100).ToString("0.00");
                dtx.Rows.Add(dr);
            }
            else
            {
                List<SeatManage.ClassModel.ReadingRoomInfo> rooms = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(roomList);

                string Title = string.Format("阅览室进出人次统计");
                var ztext = "";
                var colName = "";
                if (rbleatype == "0" || rbleatype == "EnterOutCount")
                {
                    colName = "EnterOutCount";
                }
                else
                {
                    colName = "Attendance";
                }
                sb1.Append("{title: {text: '" + Title + "'}, tooltip: {},legend: { data: [%ztext% ]}, xAxis: {axisLabel: {interval: 0,rotate:45,margin:2,textStyle:{color:\"#222\"}},");
                sb1.Append(" data: [%Date%] },");
                sb1.Append(" grid: { x: 40,x2: 100,y2: 150}, yAxis: {}, series: [%series%]}");
                int i = 0;
                var dateStr = "";
                var ztextStr = "";
                var series = "";
                foreach (SeatManage.ClassModel.ReadingRoomInfo room in rooms)
                {
                    SeatManage.ClassModel.ReadingRoomUsageStatistics roomUsage = rrsta.StatisticsDayRoomData(room, begin.Value, end.Value, ((SeatManage.ClassModel.StatisticsType)int.Parse(rbltype)));
                    DataTable dt = roomUsage.StatisticsData;
                    DataView dv = dt.DefaultView;
                    dataTables.Add(room.Name, dt);
                    if (i == 0)
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            dateStr += ("'" + r["Date"] + "'");
                            dateStr += (",");
                        }
                    }
                    series+=("{name: '"+ room.Name + "',type: 'bar', data: [");
                    foreach (DataRow r in dt.Rows)
                    {
                        series+=("" + r[colName] + "");
                        series+=(",");
                    }
                    series.Remove(series.Length - 1, 1);
                    series+=("]},");

                    ztext += "'"+room.Name+"'" + ",";
                    DataRow dr = dtx.NewRow();
                    dr["Room"] = roomUsage.RoomInfo.Name;
                    dr["EnterOutCount"] = roomUsage.EnterOurCount;
                    dr["AttendanceCount"] = (roomUsage.Attendance * 100).ToString("0.00");
                    dtx.Rows.Add(dr);
                    i++;
                }
                sb1.Replace("%Date%", dateStr.TrimEnd(','));
                sb1.Replace("%ztext%", ztext.TrimEnd(','));
                sb1.Replace("%series%", series.TrimEnd(','));
            }
            Session["DataTables"] = dataTables;
            /*if (rbleatype == "EnterOutCount")
            {
                libraryEnterOutInfo.ChartAreas[0].AxisX.Title = "阅览室";
                libraryEnterOutInfo.ChartAreas[0].AxisY.Title = "人次";
                sereo.Name = "EnterOutCount";
                sereo.LegendText = "进出人次";
                sereo.Points.DataBindXY(dtx.DefaultView, "Room", dtx.DefaultView, "EnterOutCount");
            }
            else
            {
                libraryAttendanceInfo.ChartAreas[0].AxisX.Title = "阅览室";
                libraryAttendanceInfo.ChartAreas[0].AxisY.Title = "人次";
                
                sera.Name = "EnterOutCount";
                sera.LegendText = "上座率（%）";
                sera.Points.DataBindXY(dtx.DefaultView, "Room", dtx.DefaultView, "AttendanceCount");
            }*/

            string Title1 = string.Format("阅览室进出人次统计（总数）");
            var ztext1 = "";
            var colName1 = "";
            if (rbleatype == "0" || rbleatype == "EnterOutCount")
            {
                ztext1 = "进出人次";
                colName1 = "EnterOutCount";
            }
            else
            {
                ztext1 = "上座率";
                colName1 = "AttendanceCount";
            }

            sb2.Append("{title: {text: '" + Title1 + "'}, tooltip: {},legend: { data: ['" + ztext1 + "' ]}, xAxis: {axisLabel: {interval: 0,rotate:45,margin:2,textStyle:{color:\"#222\"}},");

            sb2.Append(" data: [");
            foreach (DataRow r in dtx.Rows)
            {
                sb2.Append("'" + r["Room"] + "'");
                sb2.Append(",");
            }
            sb2.Remove(sb2.Length - 1, 1);
            sb2.Append("] }, grid: { x: 40,x2: 100,y2: 150}, yAxis: {}, series: [{name: '" + ztext1 + "',type: 'bar',");
            sb2.Append(" data: [");
            foreach (DataRow r in dtx.Rows)
            {
                sb2.Append("" + r[colName1] + "");
                sb2.Append(",");
            }
            sb2.Remove(sb2.Length - 1, 1);
            sb2.Append("]}]}");

            return Json(new { title = "", msg = "", data1 = sb1.ToString(),data2 = sb2.ToString() });
        }
        #endregion

        #region 阅览室信息统计
        public ActionResult ReadingRoomUsageInfo()
        {
            SeatManage.Bll.T_SM_Reader readerbll = new SeatManage.Bll.T_SM_Reader();
            List<string> readertypeList = readerbll.GetReaderType();
            ViewData["readertypeList"] = readertypeList;

            List<SeatManage.ClassModel.LibraryInfo> libList = SeatManage.Bll.T_SM_Library.GetLibraryInfoList(null, null, null);
            ViewData["libList"] = libList;
            return View();
        }

        public string GetReadingRoomUsageInfoData(DateTime? begin, DateTime? end,string libNo,string readerType)
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            
            if (begin >= end)
            {
                result = "结束日期必须大于等于开始日期";
                return result;
            }
            

            List<string> libNoList = new List<string>();
            libNoList.Add(libNo);
            List<SeatManage.ClassModel.ReadingRoomInfo> rooms = SeatManage.Bll.T_SM_ReadingRoom.GetReadingRooms(null, libNoList, null);

            var rrsta = new SeatManageWebV5.Code.ReadingRoomStatistics();

            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (var item in rooms)
            {
                var dt = rrsta.GetReadingRoomUsageInfo(item.No, readerType, begin.Value, end.Value);
                if (dt != null)
                {
                    sb.Append("{\"Name\": '" + dt.Rows[0].Cells[0].InnerText + "',\"No\": '" + dt.Rows[1].Cells[1].InnerText + "',\"BeginTime\": \"" + dt.Rows[2].Cells[1].InnerText +
                    "\",\"EndTime\": \"" + dt.Rows[2].Cells[3].InnerText + "\",\"otime\": \"" + dt.Rows[3].Cells[1].InnerText + "\",\"opentime\": \"" + dt.Rows[3].Cells[3].InnerText +
                    "\",\"SeatsCount\": \"" + dt.Rows[4].Cells[1].InnerText + "\",\"staListCount\": \"" + dt.Rows[4].Cells[3].InnerText + "\",\"upSeat\": \"" + dt.Rows[5].Cells[1].InnerText +
                    "\",\"avgSeat\": \"" + dt.Rows[5].Cells[3].InnerText + "\",\"userFun\": \"" + dt.Rows[6].Cells[1].InnerText + "\",\"setting\": \"" + dt.Rows[7].Cells[1].InnerText +
                    "\"}");
                    sb.Append(",");
                }
            }
            if (rooms.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            sb.Append("}");
            result = sb.ToString();
            return result.Replace("NaN%","");
        }
        #endregion

        #region 记录排行榜
        public ActionResult LogTopStatisticalV2()
        {
            return View();
        }

        public string GetLogTopStatisticalV2Data(DateTime? begin, DateTime? end, string ddllogtype, string ddlreadertype,string ddltopnum)
        {
            string result = "";
            if (begin >= end)
            {
                result = "结束日期必须大于等于开始日期";
                return result;
            }
            DataTable table = new DataTable();
            switch ((SeatManageWebV5.Code.ReadingRoomStatistics.TopLogType)int.Parse(ddltopnum))
            {
                case SeatManageWebV5.Code.ReadingRoomStatistics.TopLogType.SeatTime:
                    table = SeatManageWebV5.Code.LogTopQuery.GetSeatTimeTop(begin.Value, end.Value, int.Parse(ddlreadertype), int.Parse(ddltopnum));
                    break;
                case SeatManageWebV5.Code.ReadingRoomStatistics.TopLogType.EnterOutLog:
                    table = SeatManageWebV5.Code.LogTopQuery.GetSeatCountTop(begin.Value, end.Value, int.Parse(ddlreadertype), int.Parse(ddltopnum));
                    break;
                case SeatManageWebV5.Code.ReadingRoomStatistics.TopLogType.Blastlist:
                    table = SeatManageWebV5.Code.LogTopQuery.GetBlacklistTop(begin.Value, end.Value, int.Parse(ddlreadertype), int.Parse(ddltopnum));
                    break;
                case SeatManageWebV5.Code.ReadingRoomStatistics.TopLogType.ViolateDiscipline:
                    table = SeatManageWebV5.Code.LogTopQuery.GetViolateDisciplineTop(begin.Value, end.Value, int.Parse(ddlreadertype), int.Parse(ddltopnum));
                    break;
            }
            DataView TableView = table.DefaultView;
            Dictionary<string, DataTable> ddt = new Dictionary<string, DataTable>();
            ddt.Add("排行榜", table);
            this.Session["DataTables"] = ddt;
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (DataRow item in table.Rows)
            {
                sb.Append("{\"TopNum\": '" + item["TopNum"].ToString() + "',\"CardNo\": '" + item["CardNo"].ToString() + "',\"ReaderName\": \"" + item["ReaderName"].ToString() +
                    "\",\"TypeName\": \"" + item["TypeName"].ToString() + "\",\"DeptName\": \"" + item["DeptName"].ToString() + "\",\"LogCount\": \"" + item["LogCount"].ToString() +
                    "\",\"Remark\": \"" + item["Remark"].ToString() + "\"}");
                sb.Append(",");
            }
            if (table.Rows.Count > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            sb.Append("}");
            result = sb.ToString();
            return result.Replace("NaN%", "");
        }
        #endregion

        public void ToExcel()
        {
            if (Session["DataTables"] != null)
            {
                Dictionary<string, DataTable> dataTables = Session["DataTables"] as Dictionary<string, DataTable>;
                SeatManageWebV5.Code.DataToExcel dte = new SeatManageWebV5.Code.DataToExcel();
                dte.DataGridViewToExcel(dataTables);
            }
        }
    }
}