using SeatManage.ClassModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class SchoolInfoManageController : BaseController
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

        /// <summary>
        /// 阅览室设置界面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult ReadingRoomSetting(string id)
        {
            AuthorizeVerify.FunctionAuthorizeInfo authorize = SeatManage.Bll.AuthorizationOperation.GetFunctionAuthorize();
            SeatManage.ClassModel.ReadingRoomInfo room = SeatManage.Bll.T_SM_ReadingRoom.GetSingleRoomInfo(Request.QueryString["id"]);
            if (room == null)
            {
                room = new SeatManage.ClassModel.ReadingRoomInfo();
            }
            SeatManage.ClassModel.ReadingRoomSetting roomSet = room.Setting;
            if (roomSet == null)
            {
                roomSet = new SeatManage.ClassModel.ReadingRoomSetting();
            }
            //选座模式设置
            ViewBag.SeatSelectDefaultMode = ((int)roomSet.SeatChooseMethod.DefaultChooseMethod).ToString();
            ViewBag.SeatSelectPosChecked = roomSet.PosTimes.IsUsed;
            ViewBag.SelectSeatPosCountText = roomSet.PosTimes.Times.ToString();
            ViewBag.SelectSeatPosTimesText = roomSet.PosTimes.Minutes.ToString();
            //高级选座按钮
            ViewBag.SeatSelectAdMode = roomSet.SeatChooseMethod.UsedAdvancedSet;
            foreach (KeyValuePair<DayOfWeek, SeatChooseMethodPlan> day in roomSet.SeatChooseMethod.AdvancedSelectSeatMode)
            {
                string dayNum = ((int)day.Value.Day).ToString();
               // CheckBox DayCheck = FindControl("PanelSetting").FindControl("FormReadingRoomSet").FindControl("SeatSelectAdModeDay" + dayNum) as CheckBox;
               // DayCheck.Checked = day.Value.Used;
                for (int i = 0; i < day.Value.PlanOption.Count; i++)
                {
                    string[] begintime = day.Value.PlanOption[i].UsedTime.BeginTime.Split(':');
                    string[] endtime = day.Value.PlanOption[i].UsedTime.EndTime.Split(':');

                    //TextBox begintimeH = FindControl("PanelSetting").FindControl("FormReadingRoomSet").FindControl("SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_StartH") as TextBox;
                    //TextBox begintimeM = FindControl("PanelSetting").FindControl("FormReadingRoomSet").FindControl("SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_StartM") as TextBox;
                    //TextBox endtimeH = FindControl("PanelSetting").FindControl("FormReadingRoomSet").FindControl("SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_EndH") as TextBox;
                    //TextBox endtimeM = FindControl("PanelSetting").FindControl("FormReadingRoomSet").FindControl("SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_EndM") as TextBox;
                    //RadioButtonList selectmode = FindControl("PanelSetting").FindControl("FormReadingRoomSet").FindControl("SeatSelectAdModeDay" + dayNum + "_Time" + (i + 1) + "_SelectMode") as RadioButtonList;
                    //begintimeH.Text = begintime[0];
                    //begintimeM.Text = begintime[1];
                    //endtimeH.Text = endtime[0];
                    //endtimeM.Text = endtime[1];
                    //selectmode.SelectedValue = ((int)day.Value.PlanOption[i].ChooseMethod).ToString();
                }
            }

            return View();
        }

        /// <summary>
        /// 绑定阅览室列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ReadingRoomInfo()
        {
            List<SeatManage.ClassModel.ReadingRoomInfo> listReadingRoom = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(null);
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");
            foreach (var item in listReadingRoom)
            {
                sb.Append("{\"No\": " + item.No + ",\"Name\": \"" + item.Name + "\",\"Libaray\": \"" + item.Libaray.Name + "\",\"School\": \"" + item.Libaray.School.Name + "\"}");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append("}");
            string data = sb.ToString();
            ViewBag.Data = data;

            return View();
        }
    }
}