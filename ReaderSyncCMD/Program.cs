using SeatManage.SeatManageComm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderSyncCMD
{
    class Program
    {
        private static TimeLoop timeLoop;//循环时间  
        static string loopInterval = ConfigurationManager.AppSettings["CheckTimes"];
        static string SyncTimeHour = ConfigurationManager.AppSettings["SyncTimeHour"];
        static bool IsWork = false;

        static void Display(string msg)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + msg);
        }

        /// <summary>
        /// 判断是否时间到了指定工作时间
        /// </summary>
        /// <returns></returns>
        static bool IsTimeToWork()
        {
            bool isTrue = false;
            if (DateTime.Now.Hour.ToString() == SyncTimeHour)
            {
                isTrue = true;
            }

            return isTrue;
        }

        static void Main(string[] args)
        {
            Display("读者同步程序启动");
            int loopTime = 0;
            if (!int.TryParse(loopInterval, out loopTime))
            {
                SeatManage.SeatManageComm.WriteLog.Write("运行间隔时间获取失败，请检查是否配置了‘CheckTimes’项");
                Console.WriteLine("运行间隔时间获取失败，请检查是否配置了‘CheckTimes’项");
            }
            timeLoop = new TimeLoop(loopTime);
            timeLoop.TimeTo += new EventHandler(timeLoop_TimeTo);
            timeLoop.TimeStrat();
            Console.ReadLine();
        }

        private static void timeLoop_TimeTo(object sender, EventArgs e)
        {
            try
            {
                if (IsTimeToWork() && IsWork == false)
                {
                    IsWork = true; //开始工作
                    AddReaderInfo objAddReaderInfo = new AddReaderInfo();
                    GetReaderSource objGetReaderSource = new GetReaderSource();
                    ActiveUser objActiveUser = new ActiveUser();
                    try
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Display("清空当前读者表");
                        objAddReaderInfo.ClearDB(System.Configuration.ConfigurationManager.AppSettings["ConnectionToDB"]);
                        Display("清空当前读者表，完毕！");
                        Display("获取数据来源表数据");
                        DataTable dt = objGetReaderSource.GetReaderList();
                        Display("获取数据来源表数据一共"+dt.Rows.Count+"条");
                        SeatManage.SeatManageComm.WriteLog.Write("获取数据来源表数据一共" + dt.Rows.Count + "条");
                        Display("将数据添加到目标表");
                        objAddReaderInfo.AddNewData(dt,System.Configuration.ConfigurationManager.AppSettings["ConnectionToDB"]);
                        Display("将数据添加到目标表结束,读者表同步完毕.");
                        Display("开始激活用户");
                       int count = objActiveUser.Active();
                        Display("激活用户结束,新增了"+count+"个用户");
                        SeatManage.SeatManageComm.WriteLog.Write("激活用户结束,新增了" + count + "个用户");

                      //  Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Display(ex.ToString());
                        SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
                    }
                    IsWork = false;
                }

            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write("异常信息:" + ex);
                Display("异常信息:" + ex);
            }
        }
    }
}
