using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceDeviceAccess.WCFHost
{
    public class SpaceDeviceAccessWCFHost : IService.IService
    {
        public override string ToString()
        {
            return "空间预约系统设备获取程序";
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            Console.WriteLine("空间预约系统设备获取程序   开始工作");
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
