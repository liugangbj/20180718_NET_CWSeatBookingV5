using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace SpaceBooking.WcfHost
{
    public class SpaceBookingWcfHost : IService.IService
    {
        ServiceHost host = null;

        public override string ToString()
        {
            return "空间预约管理系统核心服务";
        }



        public void Start()
        {
            host = new ServiceHost(typeof(WCFServiceForSpaceBooking.SpaceBookingManager));
            host.Open();
            host = new ServiceHost(typeof(WCFServiceForSpaceBooking.SpaceBookingManager));
            host.Open();
        }



        public void Stop()
        {
            try
            {
                if (host != null)
                {
                    host.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            if (host != null)
            {
                host.Close();
            }
            GC.SuppressFinalize(this);
        }
    }
}
