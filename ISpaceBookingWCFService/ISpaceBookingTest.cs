using SeatManage.IWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ISpaceBookingWCFService
{
    /// <summary>
    /// 空间预约系统测试例子接口
    /// </summary>
    public partial interface ISpaceBookingTest: IExceptionService
    {
        [OperationContract]
        string  Display();
    }
}
