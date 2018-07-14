using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using SeatManage.SeatManageComm;
using System.Configuration;
using System.Drawing;

namespace SeatManageWebV2.Code
{
    /// <summary>
    /// 座位二维码生成类
    /// </summary>
    public class SeatQRCodeManager
    {

        /// <summary>
        /// 生成文字图片
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isBold"></param>
        /// <param name="fontSize"></param>
        public static Image CreateImage(string text, bool isBold, int fontSize)
        {
            int wid = 500;
            int high = 180;
            Font font;
            if (isBold)
            {
                font = new Font("Arial", fontSize, FontStyle.Bold);

            }
            else
            {
                font = new Font("Arial", fontSize, FontStyle.Regular);

            }
            //绘笔颜色
            SolidBrush brush = new SolidBrush(Color.Black);
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            Bitmap image = new Bitmap(34, 27);
            Graphics g = Graphics.FromImage(image);
            SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);//得到文本的宽高
            int width = wid;//(int)(sizef.Width + 1);
            int height = high;// (int)(sizef.Height + 1);
            image.Dispose();
            image = new Bitmap(width, height);
            g = Graphics.FromImage(image);
            g.Clear(Color.White);//透明

            RectangleF rect = new RectangleF(20, 50, width, height);
            //绘制图片
            g.DrawString(text, font, brush, rect);
            //释放对象
            g.Dispose();
            return image;
        }

        /// <summary>  
        /// 合并图片  
        /// </summary>  
        /// <param name="imgBack"></param>  
        /// <param name="img"></param>  
        /// <returns></returns>  
        public static void CombinImage(Image imgBack, Image img,string filePath, int xDeviation = 0, int yDeviation = 0)
        {

            Bitmap bmp = new Bitmap(imgBack.Width, imgBack.Height + img.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height); //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     

            //g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框    

            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);    

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2 + xDeviation, imgBack.Height + yDeviation, img.Width, img.Height);
            GC.Collect();
          //  File.Delete(filePath);
            bmp.Save(filePath);
          //  return bmp;
        }

        protected string GetDateString()
        {
            return DateTime.Now.ToString("yyyyMMddhhmmssfff");
        }

        public static void CreatSingleSeatQRCode(string readingRoomNo,string roomName,string seatNo)
        {
            try
            {
                string rootPath = System.Web.HttpContext.Current.Server.MapPath("~/SeatQRCode/");
                if (Directory.Exists(rootPath))
                {
                    string subPath = System.Web.HttpContext.Current.Server.MapPath("~/SeatQRCode/"+ readingRoomNo + "/");
                    if (!Directory.Exists(subPath))
                    {
                        Directory.CreateDirectory(subPath);
                    }
                    string schoolNo = ConfigurationManager.AppSettings["SchoolNo"].ToString();
                    string AESCode = string.Format("schoolNo={0}&readingRoomNo={1}&seatNo={2}", schoolNo, readingRoomNo, seatNo);
                    Bitmap bitmap = QRCode.GetDimensionalCode(AESAlgorithm.AESEncrypt(AESCode, "SeatManage_WeiCharCode"), 6, 8);

                    //string tempPath =  System.Web.HttpContext.Current.Server.MapPath("~/SeatQRCode/temp/GetDateString()/" + readingRoomNo + "/");
                    //Directory.CreateDirectory(tempPath);
                    //bitmap.Save(tempPath + seatNo + ".jpg");

                    string imagePath = subPath + seatNo + ".jpg";

                    

                    Image img = bitmap;
                    Image bg = CreateImage("阅览室:"+ roomName + "   座位编号："+ seatNo, true, 16);
                    CombinImage(bg, img, imagePath, 0, -50);
                    bitmap.Dispose();
                    bg.Dispose();
                    img.Dispose();
                }
                else
                {
                    WriteLog.Write("找不到" + rootPath + "文件夹，或者权限不足，请联系管理员");
                }
            }
            catch (Exception ex)
            {

                WriteLog.Write(ex.ToString());
            }
        }
    }
}