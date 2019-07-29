using ISpaceBookingWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WCFServiceForSpaceBooking
{
    public partial class SpaceBookingManager : ISpaceBookingTest
    {
        public string Display()
        {
            return "Hello world";
        }



        public void ThrowExceptionOneWay()
        {
            throw new NotImplementedException();
        }

        public void ThrowFaultEcxeption()
        {
            throw new NotImplementedException();
        }

        public void ThrowMessageFault()
        {
            throw new NotImplementedException();
        }
    }
}
