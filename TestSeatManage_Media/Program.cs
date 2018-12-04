using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSeatManage_Media
{
    class Program
    {
        static void Main(string[] args)
        {
            //seatguide01.jpg
            string MediaMd5 = SeatManage.SeatManageComm.SeatComm.GetMD5HashFromFile("seatguide01.jpg");

            Console.ReadLine();
        }
    }
}
