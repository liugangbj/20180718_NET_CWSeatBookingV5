using SeatManage.SeatManageComm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace SeatManageWebV2.FunctionPages.WechatCode
{
    public partial class MyQRCode : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string LoginID = Session["LoginID"].ToString();
                string path = Server.MapPath("~/QRCodeImages/" + LoginID + ".jpg");

                if (!File.Exists(path))
                {
                    string schoolNo = ConfigurationManager.AppSettings["SchoolNo"].ToString();
                    string AESCode = string.Format("schoolNo={0}&clientNo={1}&cardNo={2}", schoolNo, "001", LoginID);
                    Bitmap bitmap = QRCode.GetDimensionalCode(AESAlgorithm.AESEncrypt(AESCode, "SeatManage_WeiCharCode"), 6, 8);

                    // string filename = Guid.NewGuid().ToString();
                    path = Server.MapPath("~/QRCodeImages/" + LoginID + ".jpg");
                    bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                    bitmap.Dispose();
                }

                Image1.ImageUrl = "/QRCodeImages/" + LoginID + ".jpg";
            }
            catch (Exception ex)
            {
                throw new Exception("登录会话过期，请重新登录");
            }

        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    if (TextBox1.Text.Trim() == string.Empty)
        //    {
        //        Page.ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请输入你的学号！');", true);
        //    }
        //    else
        //    {
        //        string schoolNo = ConfigurationManager.AppSettings["SchoolNo"].ToString();
        //        string AESCode = string.Format("schoolNo={0}&clientNo={1}&cardNo={2}", schoolNo, "001", TextBox1.Text.Trim());
        //        Bitmap bitmap = QRCode.GetDimensionalCode(AESAlgorithm.AESEncrypt(AESCode, "SeatManage_WeiCharCode"), 6, 8);

        //        string filename = Guid.NewGuid().ToString();
        //        string path = Server.MapPath("~/QRCodeImages/"+ filename + ".jpg");
        //        bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);

        //        Image1.ImageUrl = "/QRCodeImages/"+ filename + ".jpg";
        //        //IntPtr hBitmap = bitmap.GetHbitmap();
        //        //BitmapSource bitmapImage = new BitmapImage();

        //        //try
        //        //{
        //        //    bitmapImage = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        //        //}
        //        //finally
        //        //{
        //        //    DeleteObject(hBitmap);
        //        //}
        //    }
    }

}