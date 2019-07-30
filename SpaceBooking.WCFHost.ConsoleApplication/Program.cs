using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceBooking.WCFHost.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            List<IService.IService> hosts = IService.ServiceFactory.CreateServiceAssemblys();
            if (hosts.Count == 0)
            {
                Console.WriteLine("没有需要启动的服务");
                Console.ReadLine();
                return;
            }
            foreach (IService.IService host in hosts)
            {
                try
                {
                    host.Start();
                    Console.WriteLine(string.Format("服务{0}已启动", host.ToString()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("服务{0}启动遇到异常：{1}", host.ToString(), ex.Message));
                }
            }
            //SeatManage.SeatClient.Config.Code.ProgramBackup backup = new SeatManage.SeatClient.Config.Code.ProgramBackup();
            //backup.Backup();
            Console.ReadLine();
        }
    }
}
