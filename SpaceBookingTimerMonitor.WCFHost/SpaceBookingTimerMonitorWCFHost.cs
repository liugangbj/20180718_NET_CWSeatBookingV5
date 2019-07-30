using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceBookingTimerMonitor.WCFHost
{
    public class SpaceBookingTimerMonitorWCFHost : IService.IService
    {

        public override string ToString()
        {
            return "空间预约系统定时监控服务";
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            Console.WriteLine("监控服务开始工作");
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
