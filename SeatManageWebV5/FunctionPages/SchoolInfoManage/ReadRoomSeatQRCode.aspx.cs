using ICSharpCode.SharpZipLib.Zip;
using SeatManageWebV2.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeatManageWebV2.FunctionPages.SchoolInfoManage
{
    public partial class ReadRoomSeatQRCode : BasePage
    {

        protected SeatManage.ClassModel.ReadingRoomInfo CurrentRomm
        {
            get;set;
        }

       // List<SeatManage.ClassModel.Seat> listSeat = SeatManage.Bll.T_SM_Seat.GetSeatListByRoomNum(roomNo, false);

         protected List<SeatManage.ClassModel.Seat> CurrentListSeat
        {
            get; set;
        }

        public string SubPath
        {
            get;set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string roomId = Request["id"].ToString();
            SubPath = Server.MapPath("~/SeatQRCode/" + roomId + "/");



            List<string> roomNums = new List<string>();
            roomNums.Add(roomId);
            List<SeatManage.ClassModel.ReadingRoomInfo> listReadingRoom = SeatManage.Bll.ClientConfigOperate.GetReadingRooms(roomNums);
            CurrentRomm = listReadingRoom[0];
            CurrentListSeat = SeatManage.Bll.T_SM_Seat.GetSeatListByRoomNum(roomId, false);

            //if (!Directory.Exists(subPath))
            //{
            //    Response.Write("请先生成座位二维码再查看");
            //    return;
            //}
        }

        protected string GetDateString()
        {
            return DateTime.Now.ToString("yyyyMMddhhmmssfff");
        }

        public void DownloadFile(string fileRpath)
        {

            Response.ClearHeaders();
            Response.Clear();
            Response.Expires = 0;
            Response.Buffer = true;
            Response.AddHeader("Accept-Language", "zh-tw");
            string name = System.IO.Path.GetFileName(fileRpath);
            System.IO.FileStream files = new FileStream(fileRpath, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] byteFile = null;
            if (files.Length == 0)
            {
                byteFile = new byte[1];
            }
            else
            {
                byteFile = new byte[files.Length];
            }
            files.Read(byteFile, 0, (int)byteFile.Length);
            files.Close();

            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(name, System.Text.Encoding.UTF8));
            Response.ContentType = "application/octet-stream;charset=gbk";
            Response.BinaryWrite(byteFile);
            Response.End();

        }

        /// <summary>
        /// 打包下载按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDownLoad_Click(object sender, EventArgs e)
        {
            string subPath = Server.MapPath("~/SeatQRCode/" + CurrentRomm.No + "/");
            string filePath = Server.MapPath("~/TempImageFiles/"+ GetDateString() + "/");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = filePath + CurrentRomm.No + ".zip";
            ZipHelper.CreateZip(subPath, fullPath);
            DownloadFile(fullPath);

        }
    }


}