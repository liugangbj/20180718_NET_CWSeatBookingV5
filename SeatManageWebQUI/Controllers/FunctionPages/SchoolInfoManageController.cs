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