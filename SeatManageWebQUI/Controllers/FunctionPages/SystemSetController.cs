using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SeatManageWebQUI.Controllers.FunctionPages
{
    public class SystemSetController : BaseController
    {
        // GET: SystemSet
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FunctionPagesManage()
        {
            List<SeatManage.ClassModel.SysFuncDicInfo> listSysFuncDic = new List<SeatManage.ClassModel.SysFuncDicInfo>();
            SeatManage.Bll.SysFuncDic bllSysFuncDic = new SeatManage.Bll.SysFuncDic();

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"form.paginate.pageNo\": 1,");
            sb.Append("\"form.paginate.totalRows\": 100,");
            sb.Append("	\"rows\": [");

            listSysFuncDic = bllSysFuncDic.GetFuncPage(null, null);
            foreach (var item in listSysFuncDic)
            {
                sb.Append("{\"ModSeq\": "+item.No+",\"MCaption\": \""+item.Name+"\",\"MenuLink\": \""+ item.PageUrl+ "\",\"OrderSeq\": \""+ item.Order + "\"}");
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append("}");
            string data = sb.ToString();
            ViewBag.Data = data;
            return View();
        }




        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ModSeq"></param>
        /// <returns></returns>
        public JsonResult RemoveItem(string ModSeq)
        {
            JsonResult result = null;

            SeatManage.ClassModel.SysFuncDicInfo modelSysFuncDicInfo = new SeatManage.ClassModel.SysFuncDicInfo();
            SeatManage.Bll.SysFuncDic bllSysFuncDic = new SeatManage.Bll.SysFuncDic();

            modelSysFuncDicInfo.No = ModSeq;
            bool isTrue = bllSysFuncDic.DeleteFuncPage(modelSysFuncDicInfo);
            if (isTrue)
            {
                result = Json(new { status = "yes", message = "删除成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                result = Json(new { status = "no", message = "删除失败" }, JsonRequestBehavior.AllowGet);
            }

            return result;
        }



        public ActionResult MenuManage()
        {
            return View();
        }
    }
}